using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.PaymentRequestDto
{
    public class PaymentRequestStatusRequest
    {
        public PaymentStatus Status { get;  set; }
        public string? AdminNote { get;  set; }
    }
}
