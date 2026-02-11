using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.SpecsDtos;
using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class VehicleAdDetailsResponse
    {
        private VehicleAd VehicleAd;

        public VehicleAdDetailsResponse(VehicleAd vehicleAd, string? currentUserId = null)
        {
            VehicleAd = vehicleAd;
            IsFavorite = currentUserId != null && vehicleAd.UserFavorites.Any(f => f.UserId == currentUserId);
        }
          public bool IsFavorite { get; set; }
        public int Id => VehicleAd.Id;
        public CityResponse? City => new (VehicleAd.City??new City());
        public ManufacturerModelResponse? VehicleModel => new ManufacturerModelResponse(VehicleAd.VehicleModel ?? new VehicleModel());
        public int ManufactureYear => VehicleAd.ManufactureYear;
        public string? Thumbnail => VehicleAd.Thumbnail;
        public bool IsUsed => VehicleAd.IsUsed;
        public FuelType FuelType => VehicleAd.FuelType;
        public bool Installment => VehicleAd.Installment;
        public decimal Price => VehicleAd.Price;
        public Category Category  => VehicleAd.Category;
        public double UsedKilometers => VehicleAd.UsedKilometers;
        public string Color => VehicleAd.Color;
        public string Description => VehicleAd.Description;
        public string Name => VehicleAd.User?.Profile?.Name??"";
        public SellerType SellerType => VehicleAd.User?.Profile?.SellerType ?? SellerType.Owner;
        public string PhoneNumber => VehicleAd.User?.Profile?.PhoneNumber ?? "";
        public string Slug => VehicleAd.Slug;
        public CarSpecsResponse? CarSpecs => Category == Category.Passenger&& VehicleAd.CarSpecs !=null? new CarSpecsResponse(VehicleAd.CarSpecs):null;
        public TruckSpecsResponse? TruckSpecs => Category == Category.Trucks && VehicleAd.TruckSpecs != null ? new TruckSpecsResponse(VehicleAd.TruckSpecs) : null;
        public MotorSpecsResponse? MotorSpecs => Category == Category.Motorcycles && VehicleAd.MotorSpecs != null ? new MotorSpecsResponse(VehicleAd.MotorSpecs) : null;
        public List<string> Gallery => VehicleAd.VehicleImages.Select(x=>x.ImagePath).ToList();
        public int ViewsCount => VehicleAd.ViewsCount;
        public int FavoritesCount => VehicleAd.FavoritesCount;
        public DateTime PostDate => VehicleAd.PublishedAt ?? DateTime.UtcNow;

    }
}
