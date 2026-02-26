using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class PaymentGateway
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; 
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string AccountNumber { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
