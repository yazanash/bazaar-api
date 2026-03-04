using Azure.Core;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Services;
using Bazaar.Entityframework;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyAdsController : ControllerBase
    {
        private readonly WebPImageService _imageService;
        private readonly IUserAdDataService _adDataService;
        private readonly IDataService<City> _cityDataService;
        private readonly IDataService<Manufacturer> _manufacturerDataService;
        private readonly IDataService<VehicleModel> _vehicleModelDataService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVehicleImageDataService _vehicleImageDataService;
        private readonly IUserWalletService _userWalletService ;
        private readonly IProfileDataService _profileDataService;
        public MyAdsController(WebPImageService imageService, IUserAdDataService adDataService, IDataService<City> cityDataService, IDataService<Manufacturer> manufacturerDataService, IDataService<VehicleModel> vehicleModelDataService, IWebHostEnvironment webHostEnvironment, IVehicleImageDataService vehicleImageDataService, IUserWalletService userWalletService, IProfileDataService profileDataService)
        {
            _imageService = imageService;
            _adDataService = adDataService;
            _cityDataService = cityDataService;
            _manufacturerDataService = manufacturerDataService;
            _vehicleModelDataService = vehicleModelDataService;
            _webHostEnvironment = webHostEnvironment;
            _vehicleImageDataService = vehicleImageDataService;
            _userWalletService = userWalletService;
            _profileDataService = profileDataService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAd([FromBody] VehicleAdRequest adRequest)
        {

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var wallet = await _userWalletService.GetUserWallet(userId);
            if (wallet.AdsLimit==0 || wallet.ExpiryDate < DateTime.UtcNow)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    errorCode = "INSUFFICIENT_CREDIT",
                    message = "لا يوجد رصيد كافٍ في محفظتك أو أن اشتراكك قد انتهى."
                });
            }
            var city = await _cityDataService.GetByIdAsync(adRequest.CityId);
            var model = await _vehicleModelDataService.GetByIdAsync(adRequest.VehicleModelId);
            var manufcturer = await _manufacturerDataService.GetByIdAsync(model.ManufacturerId);

            VehicleAd vehicleAd = adRequest.ToModel();
            vehicleAd.UserId = userId;
            vehicleAd.GenerateSlug(manufcturer.Name, model.Name, city.EnglishName, adRequest.ManufactureYear);
            VehicleAd createdAd = await _adDataService.CreateAsync(vehicleAd);

            List<VehicleImage> gallery = ProcessAdImages(vehicleAd, adRequest);
            await _vehicleImageDataService.SyncImagesAndGetDeletablesAsync(createdAd.Id, gallery);
            await _userWalletService.ConsumeAdCredit(userId, createdAd.Id);
            return Ok(new { slug = createdAd.Slug });


        }
        [HttpGet]
        public async Task<IActionResult> GetMyAds([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            PagedList<VehicleAd> myads = await _adDataService.GetUserAdsAsync(userId, pageNumber, pageSize);
            PagedList<UserAdResponse> response = myads.MapTo(x => new UserAdResponse
            {
                VehicleAdResponse = new VehicleAdResponse(x),
                PubStatus = x.PublishStatus,
                Reasone = x.RejectionReason

            });
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdDetails(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            VehicleAd? vehicleAd = await _adDataService.GetByIdAsync(id);
            if (vehicleAd == null) return NotFound("Ad Not Found");
            return Ok(new VehicleAdEditResponse(vehicleAd));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMyAd(int id, [FromBody] VehicleAdRequest adRequest)
        {
            var city = await _cityDataService.GetByIdAsync(adRequest.CityId);
            var model = await _vehicleModelDataService.GetByIdAsync(adRequest.VehicleModelId);
            var manufcturer = await _manufacturerDataService.GetByIdAsync(model.ManufacturerId);
            VehicleAd vehicleAd = adRequest.ToModel();
            vehicleAd.GenerateSlug(manufcturer.Name, model.Name, city.EnglishName, adRequest.ManufactureYear);
            vehicleAd.Id = id;
            VehicleAd updatedAd = await _adDataService.UpdateAsync(vehicleAd);

            List<VehicleImage> gallery = ProcessAdImages(vehicleAd, adRequest);
            List<string> imagesToDelete = await _vehicleImageDataService.SyncImagesAndGetDeletablesAsync(updatedAd.Id, gallery);
            foreach(string img in imagesToDelete)
            {
                _imageService.DeleteImage(img);
            }
            return Ok(new { slug = updatedAd.Slug });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyAd(int id)
        {
            VehicleAd vehicleAd =await _adDataService.GetByIdAsync(id);
            bool deleted = await _adDataService.DeleteAsync(id);
            foreach(var img in vehicleAd.VehicleImages)
            {
                _imageService.DeleteImage(img.ImagePath);
            }
            return Ok(deleted);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> Upload([FromForm] UploadImageRequest uploadImageRequest)
        {

            if (uploadImageRequest.Image == null || uploadImageRequest.Image.Length == 0)
                return BadRequest("No image uploaded.");
            string uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}";
            string imageTempUrl = await _imageService.SaveImageAsWebP(uploadImageRequest.Image, "temp", uniqueFileName);
            return Ok(new { ImageUrl = imageTempUrl });
        }
        [HttpPost("star-ad/{id}")]
        public async Task<IActionResult> StarAd(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var wallet = await _userWalletService.GetUserWallet(userId);
            if (wallet.FeatureLimits == 0 || wallet.ExpiryDate < DateTime.UtcNow)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    errorCode = "INSUFFICIENT_CREDIT",
                    message = "لا يوجد رصيد كافٍ في محفظتك أو أن اشتراكك قد انتهى."
                });
            }
            bool starred = await _adDataService.StarAdAsync(id);
            await _userWalletService.ConsumeFeatureCredit(userId, id);
            return Ok(new { starred });
        }
        [HttpGet("validate")]
        [Authorize]
        public async Task<IActionResult> ValidateAdPosting()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var profile = await _profileDataService.GetUserProfileAsync(userId);
            bool hasProfile = profile != null;
            var wallet = await _userWalletService.GetUserWallet(userId);
            bool hasActivePackage = wallet.AdsLimit > 0 && wallet.ExpiryDate > DateTime.UtcNow;

            return Ok(new
            {
                canPost = hasProfile && hasActivePackage,
                hasProfile = hasProfile,
                hasActivePackage = hasActivePackage,
                remainingAds = wallet.AdsLimit,
                expiryDate = wallet.ExpiryDate
            });
        }
        private List<VehicleImage> ProcessAdImages(VehicleAd ad, VehicleAdRequest request)
        {
            List<VehicleImage> gallery = new List<VehicleImage>();
            if (request.Gallery?.Any() == true)
            {
                for (int i = 0; i < request.Gallery.Count; i++)
                {
                    var vehicleImage = request.Gallery[i];
                    var path = vehicleImage.ImageUrl;
                    if (path.StartsWith("temp"))
                    {
                        string? savedName = ProcessAdImage(path, ad.Slug);
                        if (savedName != null)
                            gallery.Add(new VehicleImage { VehicleId = ad.Id, ImagePath = savedName, Order = i, Id = vehicleImage.Id });
                    }
                    else
                    {
                        gallery.Add(new VehicleImage { VehicleId = ad.Id, ImagePath = path, Order = i, Id = vehicleImage.Id });
                    }
                }
            }
            return gallery;
        }
        private string? ProcessAdImage(string Image, string slug)
        {
            if (!string.IsNullOrEmpty(Image) && Image.StartsWith("temp"))
            {
                var tempPath = Image;
                var fileName = Path.GetFileName(tempPath);
                var uniqueSuffix = DateTime.UtcNow.Ticks.ToString().Substring(10);
                var extension = Path.GetExtension(tempPath);
                var savedName = $"ads/{slug}-{uniqueSuffix}{extension}";
                _imageService.MoveFile(tempPath, savedName);
                return savedName;
            }
            return Image;
        }
    }
}
