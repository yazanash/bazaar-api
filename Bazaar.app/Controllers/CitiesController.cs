using Bazaar.app.Dtos;
using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Helpers;
using Bazaar.Entityframework;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IDataService<City> _dataService;

        public CitiesController(IDataService<City> dataService)
        {
            _dataService = dataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            IEnumerable<City> cities = await _dataService.GetAllAsync();
            IEnumerable<CityResponse> response = cities.Select(c => new CityResponse(c)).ToList();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CityRequest cityRequest)
        {
            City city = cityRequest.ToModel();
            City createdCity = await _dataService.CreateAsync(city);
            return Ok(new CityResponse(createdCity));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] CityRequest cityRequest)
        {
            City city = cityRequest.ToModel();
            city.Id = id;
            City updatedCity = await _dataService.UpdateAsync(city);
            return Ok(new CityResponse(updatedCity));
        }
    }
}
