using Bazaar.app.Dtos.ManufacturerDto;
using Bazaar.app.Dtos.PaymentGatewayDtos;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewaysController : ControllerBase
    {
        private readonly IPaymentGatewayDataService _dataService;

        public PaymentGatewaysController(IPaymentGatewayDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetGateways()
        {
            IEnumerable<PaymentGateway> gateways = await _dataService.GetAllAsync();
            IEnumerable<PaymentGatewayResponse> response = gateways.Select(c => new PaymentGatewayResponse(c)).ToList();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGateways([FromBody] PaymentGatewayRequest paymentGatewayRequest)
        {
            PaymentGateway paymentGateway = paymentGatewayRequest.ToModel();
            PaymentGateway createdPaymentGateway = await _dataService.CreateAsync(paymentGateway);
            return Ok(new PaymentGatewayResponse(createdPaymentGateway));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGateways(int id, [FromBody] PaymentGatewayRequest paymentGatewayRequest)
        {
            PaymentGateway paymentGateway = paymentGatewayRequest.ToModel();
            paymentGateway.Id = id;
            PaymentGateway updatedPaymentGateway = await _dataService.UpdateAsync(paymentGateway);
            return Ok(new PaymentGatewayResponse(updatedPaymentGateway));
        }
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleGateway(int id)
        {
            bool toggled = await _dataService.ToggleStatusAsync(id);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGateway(int id)
        {
            bool toggled = await _dataService.DeleteAsync(id);
            return Ok();
        }
    }
}
