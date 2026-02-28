using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.ManufacturerDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IDataService<Manufacturer> _dataService;

        public ManufacturersController(IDataService<Manufacturer> dataService)
        {
            _dataService = dataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            IEnumerable<Manufacturer> manufacturers = await _dataService.GetAllAsync();
            IEnumerable<ManufacturerResponse> response = manufacturers.Select(c => new ManufacturerResponse(c)).ToList();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturerById(int id)
        {
            Manufacturer manufacturer = await _dataService.GetByIdAsync(id);
            ManufacturerModelsResponse response =new ManufacturerModelsResponse(manufacturer);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateManufacturer([FromBody] ManufacturerRequest manufacturerRequest)
        {
            Manufacturer city = manufacturerRequest.ToModel();
            Manufacturer createdManufacturer = await _dataService.CreateAsync(city);
            return Ok(new ManufacturerResponse(createdManufacturer));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManufacturer(int id, [FromBody] ManufacturerRequest manufacturerRequest)
        {
            Manufacturer manufacturer = manufacturerRequest.ToModel();
            manufacturer.Id = id;
            Manufacturer updatedManufacturer = await _dataService.UpdateAsync(manufacturer);
            return Ok(new ManufacturerResponse(updatedManufacturer));
        }
    }
}
