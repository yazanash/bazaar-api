using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelsController : ControllerBase
    {
        private readonly IDataService<VehicleModel> _dataService;

        public VehicleModelsController(IDataService<VehicleModel> dataService)
        {
            _dataService = dataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetVehicleModels()
        {
            IEnumerable<VehicleModel> vehicleModels = await _dataService.GetAllAsync();
            IEnumerable<VehicleModelResponse> response = vehicleModels.Select(c => new VehicleModelResponse(c)).ToList();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicleModel([FromBody] VehicleModelRequest vehicleModelRequest)
        {
            VehicleModel vehicleModel = vehicleModelRequest.ToModel();
            VehicleModel createdVehicleModel = await _dataService.CreateAsync(vehicleModel);
            return Ok(new VehicleModelResponse(createdVehicleModel));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleModel(int id, [FromBody] VehicleModelRequest vehicleModelRequest)
        {
            VehicleModel vehicleModel = vehicleModelRequest.ToModel();
            vehicleModel.Id = id;
            VehicleModel updatedVehicleModel = await _dataService.UpdateAsync(vehicleModel);
            return Ok(new VehicleModelResponse(updatedVehicleModel));
        }
    }
}
