using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.PackageDtos
{
    public class PackageRequest
    {
        public string Name { get; set; } = string.Empty;
        public int AdLimits { get; set; }
        public int FeaturedLimit { get; set; }
        public decimal Price { get; set; }
        public Package ToModel()
        {
            return new Package
            {
                Name = Name,
                AdLimits = AdLimits,
                FeaturedLimit = FeaturedLimit,
                Price = Price
            };
        }
    }
}
