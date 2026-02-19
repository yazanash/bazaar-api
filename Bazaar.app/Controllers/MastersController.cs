using Bazaar.app.Dtos;
using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.ManufacturerDto;
using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersController : ControllerBase
    {
        private readonly IDataService<VehicleModel> _vehicleModelDataService;
        private readonly IDataService<City> _cityDataService;
        private readonly IDataService<Manufacturer> _manufcturerDataService;

        public MastersController(IDataService<VehicleModel> vehicleModelDataService, IDataService<City> cityDataService, IDataService<Manufacturer> manufcturerDataService)
        {
            _vehicleModelDataService = vehicleModelDataService;
            _cityDataService = cityDataService;
            _manufcturerDataService = manufcturerDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetVehicleModels()
        {
            MasterResponse masterResponse = new MasterResponse();
            IEnumerable<VehicleModel> vehicleModels = await _vehicleModelDataService.GetAllAsync();
            masterResponse.Models = vehicleModels.Select(c => new ManufacturerModelResponse(c)).OrderBy(r => r.Name).ToList();

            IEnumerable<City> cities = await _cityDataService.GetAllAsync();
            masterResponse.Cities = cities.Select(c => new CityResponse(c)).ToList();

            IEnumerable<Manufacturer> manufacturers = await _manufcturerDataService.GetAllAsync();
            masterResponse.Manufacturer = manufacturers.Select(c => new ManufacturerResponse(c)).ToList();

            return Ok(masterResponse);
        }
    }
}
