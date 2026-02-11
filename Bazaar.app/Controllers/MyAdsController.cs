using Azure.Core;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Services;
using Bazaar.Entityframework;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public MyAdsController(WebPImageService imageService, IUserAdDataService adDataService, IDataService<City> cityDataService, IDataService<Manufacturer> manufacturerDataService, IDataService<VehicleModel> vehicleModelDataService)
        {
            _imageService = imageService;
            _adDataService = adDataService;
            _cityDataService = cityDataService;
            _manufacturerDataService = manufacturerDataService;
            _vehicleModelDataService = vehicleModelDataService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAd([FromForm] VehicleAdRequest adRequest)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var city = await _cityDataService.GetByIdAsync(adRequest.CityId);
            var model = await _vehicleModelDataService.GetByIdAsync(adRequest.VehicleModelId);
            var manufcturer = await _manufacturerDataService.GetByIdAsync(model.ManufacturerId);

            VehicleAd vehicleAd = adRequest.ToModel();
            vehicleAd.UserId = userId;
            vehicleAd.GenerateSlug(manufcturer.Name, model.Name, city.EnglishName, adRequest.ManufactureYear);
            await ProcessAdImages(vehicleAd, adRequest);
            VehicleAd createdAd = await _adDataService.CreateAsync(vehicleAd);
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
            return Ok(new VehicleAdDetailsResponse(vehicleAd));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMyAd(int id, [FromForm] VehicleAdRequest adRequest)
        {
            var city = await _cityDataService.GetByIdAsync(adRequest.CityId);
            var model = await _vehicleModelDataService.GetByIdAsync(adRequest.VehicleModelId);
            var manufcturer = await _manufacturerDataService.GetByIdAsync(model.ManufacturerId);
            VehicleAd vehicleAd = adRequest.ToModel();
            vehicleAd.GenerateSlug(manufcturer.Name, model.Name, city.EnglishName, adRequest.ManufactureYear);
            await ProcessAdImages(vehicleAd, adRequest);
            vehicleAd.Id = id;
            VehicleAd updatedAd = await _adDataService.UpdateAsync(vehicleAd);
            return Ok(new { slug = updatedAd.Slug });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyAd(int id)
        {
            bool deleted = await _adDataService.DeleteAsync(id);
            return Ok(deleted);
        }
        private async Task ProcessAdImages(VehicleAd ad, VehicleAdRequest request)
        {
            if (request.Thumbnail != null)
            {
                ad.Thumbnail = await _imageService.SaveImageAsWebP(request.Thumbnail, "ads", $"{ad.Slug}-main");
            }

            if (request.Gallery?.Any() == true)
            {
                ad.VehicleImages.Clear();
                for (int i = 0; i < request.Gallery.Count; i++)
                {
                    string name = $"{ad.Slug}-{i + 1}";
                    string savedName = await _imageService.SaveImageAsWebP(request.Gallery[i], "ads", name);
                    ad.VehicleImages.Add(new VehicleImage { ImagePath = savedName });
                }
            }
        }
    }
}
