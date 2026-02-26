using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class PaymentRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int PackageId { get; set; }
        public string? PackageName { get; set; }
        public decimal PackagePrice { get; set; }
        public int PaymentGatewayId { get; set; }
        public string? PaymentGatewayName { get; set; }
        public string ReceiptImagePath { get; set; } = string.Empty; 
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? AdminNote { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserEmail { get; set; } = string.Empty;
    }
}
