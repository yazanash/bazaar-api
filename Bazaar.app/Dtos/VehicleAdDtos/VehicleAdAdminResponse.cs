using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.SpecsDtos;
using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class VehicleAdAdminResponse
    {
        private VehicleAd VehicleAd;

        public VehicleAdAdminResponse(VehicleAd vehicleAd)
        {
            VehicleAd = vehicleAd;
        }
        public int Id => VehicleAd.Id;
        public ManufacturerModelResponse? VehicleModel => new ManufacturerModelResponse(VehicleAd.VehicleModel ?? new VehicleModel());
        public decimal Price => VehicleAd.Price;
        public Category Category => VehicleAd.Category;
        public string Description => VehicleAd.Description;
        public string Name => VehicleAd.User?.Profile?.Name ?? "";
        public SellerType SellerType => VehicleAd.User?.Profile?.SellerType ?? SellerType.Owner;
        public string PhoneNumber => VehicleAd.User?.Profile?.PhoneNumber ?? "";
        public string Slug => VehicleAd.Slug;
        public DateTime PostDate => VehicleAd.PublishedAt ?? DateTime.UtcNow;
        public PubStatus Status => VehicleAd.PublishStatus;
    }
}
