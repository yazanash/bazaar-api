using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class AdBanners
    {
        public int Id {get;set; }
        public string ImageUrl {get;set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public DateTime ActivationDate { get; set; } 
    }
}
