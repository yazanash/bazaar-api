using Bazaar.app.Dtos;
using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Services;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdBannersController : ControllerBase
    {
        private readonly IDataService<AdBanners> _dataService;
        private readonly WebPImageService _webPImageService;

        public AdBannersController(IDataService<AdBanners> dataService, WebPImageService webPImageService)
        {
            _dataService = dataService;
            _webPImageService = webPImageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAdBanners()
        {
            IEnumerable<AdBanners> banners = await _dataService.GetAllAsync();
            var response = banners.Select(x=>new AdBannerResponse(x)).ToList();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdBanners([FromForm] AdBannerRequest adBannerRequest)
        {
            if (adBannerRequest == null || adBannerRequest.ImageUrl == null || adBannerRequest.ImageUrl.Length == 0) return BadRequest();

            AdBanners adBanner = adBannerRequest.ToModel();
            string fileName = $"{Guid.NewGuid()}";
            adBanner.ImageUrl = await _webPImageService.SaveImageAsWebP(adBannerRequest.ImageUrl,"banners",fileName);
            AdBanners createdAdBanner = await _dataService.CreateAsync(adBanner);
            return Ok(new AdBannerResponse(createdAdBanner));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdBanners(int id, [FromForm] AdBannerRequest adBannerRequest)
        {
            if (adBannerRequest == null ) return BadRequest();

            AdBanners oldBanner = await _dataService.GetByIdAsync(id);
            oldBanner.ActivationDate =adBannerRequest.ActivationDate;
            oldBanner.ExpirationDate = adBannerRequest.ActivationDate.AddDays(adBannerRequest.DurationDays);
            oldBanner.Link = adBannerRequest.Link;
            if (adBannerRequest.ImageUrl != null && adBannerRequest.ImageUrl.Length != 0)
            {
                if (oldBanner.ImageUrl != null)
                {
                    _webPImageService.DeleteImage(oldBanner.ImageUrl);
                }
                string fileName = $"{Guid.NewGuid()}";
                oldBanner.ImageUrl = await _webPImageService.SaveImageAsWebP(adBannerRequest.ImageUrl, "banners", fileName);
            }
            AdBanners updatedAdBanner = await _dataService.UpdateAsync(oldBanner);
            return Ok(new AdBannerResponse(updatedAdBanner));
        }
    }
}
