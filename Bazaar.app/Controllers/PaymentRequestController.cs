using Bazaar.app.Dtos;
using Bazaar.app.Dtos.PaymentRequestDto;
using Bazaar.app.Dtos.VehicleAdDtos;
using Bazaar.app.Services;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentRequestController : ControllerBase
    {
        private readonly IPaymentRequestService _paymentRequestService;
        private readonly IUserWalletService _userWalletService;
        private readonly TelegramBotService _telegramBotService;
        public PaymentRequestController(IPaymentRequestService paymentRequestService, IUserWalletService userWalletService, TelegramBotService telegramBotService)
        {
            _paymentRequestService = paymentRequestService;
            _userWalletService = userWalletService;
            _telegramBotService = telegramBotService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAdDetails()
        {
            IEnumerable<PaymentRequest> requests = await _paymentRequestService.GetPendingRequestsAsync();
            var response = requests.Select(x => new PaymentRequestResponse(x)).ToList();
            return Ok(response);
        }

        [HttpPost("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] PaymentRequestStatusRequest paymentRequestStatusRequest)
        {
            await _paymentRequestService.UpdateStatusAsync(id, paymentRequestStatusRequest.Status, paymentRequestStatusRequest.AdminNote);
            PaymentRequest paymentRequest = await _paymentRequestService.GetPaymentRequest(id);
            if (paymentRequest.Status == PaymentStatus.Accepted)
            {
                await _userWalletService.CreatePackageBundle(paymentRequest.UserId, paymentRequest.PackageId);
                await _telegramBotService.HandelRequestAccepted(paymentRequest.ChatId);
            }
            else
            {
                await _telegramBotService.HandelRequestDenied(paymentRequest.ChatId, paymentRequest.AdminNote ?? "يمكنك التواصل مع فريق bazaar 963 لمعرفة سبب الرفض");
            }
            return Ok();
        }
    }
}
