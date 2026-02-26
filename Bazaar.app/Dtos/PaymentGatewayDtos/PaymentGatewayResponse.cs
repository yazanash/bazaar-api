using Bazaar.Entityframework.Models;
using System.ComponentModel.DataAnnotations;

namespace Bazaar.app.Dtos.PaymentGatewayDtos
{
    public class PaymentGatewayResponse
    {
        private PaymentGateway PaymentGateway;

        public PaymentGatewayResponse(PaymentGateway paymentGateway)
        {
            PaymentGateway = paymentGateway;
        }
        public int Id => PaymentGateway.Id;
        public string Name => PaymentGateway.Name;
        public string AccountNumber => PaymentGateway.AccountNumber;
        public string? Instructions => PaymentGateway.Instructions;
        public bool IsActive => PaymentGateway.IsActive;
        public DateTime CreatedAt => PaymentGateway.CreatedAt;
    }
}
