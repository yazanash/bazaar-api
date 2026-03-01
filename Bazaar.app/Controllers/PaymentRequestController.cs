using Bazaar.app.Dtos;
using Bazaar.app.Dtos.PaymentRequestDto;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.Entityframework.Models;
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
    public class PaymentRequestController : ControllerBase
    {
        private readonly IPaymentRequestService _paymentRequestService;

        public PaymentRequestController(IPaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAdDetails()
        {
            IEnumerable<PaymentRequest> requests = await _paymentRequestService.GetPendingRequestsAsync();
            var response = requests.Select(x=>new PaymentRequestResponse(x)).ToList();
            return Ok(response);
        }

        [HttpPost("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id ,[FromBody] PaymentRequestStatusRequest paymentRequestStatusRequest)
        {
            await _paymentRequestService.UpdateStatusAsync(id, paymentRequestStatusRequest.Status, paymentRequestStatusRequest.AdminNote);
            return Ok();
        }
    }
}
