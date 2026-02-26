using Bazaar.app.Dtos;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Helpers;
using Bazaar.Entityframework;
using Bazaar.Entityframework.Models.Stats;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsDataService _statsDataService;

        public StatsController(IStatsDataService statsDataService)
        {
            _statsDataService = statsDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           DashboardStats stats = await _statsDataService.GetStatsAsync();
            return Ok(stats);
        }
    }
}
