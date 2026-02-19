using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.PackageDtos
{
    public class PackageResponse
    {
        private Package Package;

        public PackageResponse(Package package)
        {
            Package = package;
        }
        public int Id => Package.Id;
        public string Name => Package.Name;
        public int AdLimits => Package.AdLimits;
        public int FeaturedLimit => Package.FeaturedLimit;
        public decimal Price => Package.Price;
    }
}
