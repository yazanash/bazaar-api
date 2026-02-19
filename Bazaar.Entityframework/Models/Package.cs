using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AdLimits { get; set; }
        public int FeaturedLimit { get; set; }
        public decimal Price { get; set; }

        internal void MergeWith(Package entity)
        {
            Name = entity.Name;
            AdLimits = entity.AdLimits;
            FeaturedLimit = entity.FeaturedLimit;
            Price = entity.Price;
        }
    }
}
