using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.PaymentRequestDto
{
    public class PaymentRequestResponse
    {
        private PaymentRequest PaymentRequest;

        public PaymentRequestResponse(PaymentRequest paymentRequest)
        {
            PaymentRequest = paymentRequest;
        }
        public int Id => PaymentRequest.Id;
        public string UserId => PaymentRequest.UserId;
        public int PackageId => PaymentRequest.PackageId;
        public string? PackageName => PaymentRequest.PackageName;
        public decimal PackagePrice => PaymentRequest.PackagePrice;
        public int PaymentGatewayId => PaymentRequest.PaymentGatewayId;
        public string? PaymentGatewayName => PaymentRequest.PaymentGatewayName;
        public string ReceiptImagePath => PaymentRequest.ReceiptImagePath;
        public PaymentStatus Status => PaymentRequest.Status;
        public string? AdminNote => PaymentRequest.AdminNote;
        public DateTime CreatedAt => PaymentRequest.CreatedAt;
        public string UserEmail => PaymentRequest.UserEmail;
    }
}
