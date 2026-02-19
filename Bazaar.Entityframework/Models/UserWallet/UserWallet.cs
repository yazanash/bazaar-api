using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.UserWallet
{
    public class UserWallet
    {
        [Key]
        public string UserId { get; set; } =string.Empty;
        public int AdsLimit { get; set; }
        public int FeatureLimits {  get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
