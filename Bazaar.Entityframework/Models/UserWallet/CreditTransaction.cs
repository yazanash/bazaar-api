using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.UserWallet
{
    public class CreditTransaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int AdId { get; set; }
        public TransactionType Type { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
