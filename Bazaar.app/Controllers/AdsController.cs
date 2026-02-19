using Bazaar.app.Dtos;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Helpers;
using Bazaar.Entityframework;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdDataService _adDataService;
      

        public AdsController(IAdDataService adDataService)
        {
            _adDataService = adDataService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetAds([ModelBinder(BinderType = typeof(SearchRequestBinder))] SearchRequest request)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PagedList<VehicleAd> pagedList = await _adDataService.SearchAsync(request.General, request.Specs, request.pageNumber, request.PageSize, userId);
            PagedList <VehicleAdResponse> response = pagedList.MapTo(ad => new VehicleAdResponse(ad, userId));
            return Ok(response);
        }
       
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAdsByStatus([FromQuery] GetAdsByStatusQueryParams byStatusQueryParams)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PagedList<VehicleAd> pagedList = await _adDataService.GetByPostStatusAsync(byStatusQueryParams.PageNumber, byStatusQueryParams.PageSize, userId, byStatusQueryParams.PubStatus);
            PagedList<VehicleAdResponse> response = pagedList.MapTo(ad => new VehicleAdResponse(ad, userId));
            return Ok(response);
        }
        [HttpGet("home")]
        public async Task<IActionResult> GetHomeAds(int pageNumber  = 1, int PageSize  = 20)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PagedList<VehicleAd> pagedList = await _adDataService.GetAllStarredAsync( pageNumber, PageSize, userId);
            PagedList<VehicleAdResponse> response = pagedList.MapTo(ad => new VehicleAdResponse(ad, userId));
            return Ok(response);
        }
        [HttpGet("ad/{slug}")]
        public async Task<IActionResult> GetAdDetails(string slug)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            VehicleAd? vehicleAd = await _adDataService.GetBySlugAsync(slug, userId);
            if (vehicleAd == null) return NotFound("Ad Not Found");
            return Ok(new VehicleAdDetailsResponse(vehicleAd, userId));
        }
       
        [HttpPost("like")]
        public async Task<IActionResult> LikeToggle([FromBody] LikeRequest likeRequest)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            await _adDataService.ToggleVehicleFavoriteAsync(likeRequest.Id, userId);
            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> ChangeAdStatus(int id, [FromBody] AdStatusRequest adStatusRequest)
        {
            await _adDataService.ChangeAddStatus(id, adStatusRequest.PubStatus, adStatusRequest.Reasone);
            return Ok();
        }
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritesAds(int pageNumber = 1, int PageSize = 20)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();
                PagedList<VehicleAd> pagedList = await _adDataService.GetUserFavoritesAsync(pageNumber, PageSize, userId);
                PagedList<VehicleAdResponse> response = pagedList.MapTo(ad => new VehicleAdResponse(ad, userId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
