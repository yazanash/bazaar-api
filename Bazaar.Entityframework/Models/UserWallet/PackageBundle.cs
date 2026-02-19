using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.UserWallet
{
    public class PackageBundle
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int AdsLimit { get; set; }
        public int FeaturedLimit { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
