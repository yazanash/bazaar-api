using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.PaymentGatewayDtos
{
    public class PaymentGatewayRequest
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public bool IsActive { get; set; } = true;
        public PaymentGateway ToModel()
        {
            return new PaymentGateway { Name = Name, AccountNumber = AccountNumber, Instructions = Instructions, IsActive = IsActive };
        }
    }
}
