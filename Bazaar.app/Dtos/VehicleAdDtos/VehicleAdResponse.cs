using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.SpecsDtos;
using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class VehicleAdResponse
    {
        private VehicleAd VehicleAd;

        public VehicleAdResponse(VehicleAd vehicleAd, string? currentUserId = null)
        {
            VehicleAd = vehicleAd;
            IsFavorite = currentUserId != null && vehicleAd.UserFavorites.Any(f => f.UserId == currentUserId);
        }
        public int Id => VehicleAd.Id;
        public bool IsFavorite { get; set; }
        public CityResponse? City => new(VehicleAd.City ?? new City());
        public ManufacturerModelResponse? VehicleModel => new ManufacturerModelResponse(VehicleAd.VehicleModel ?? new VehicleModel());
        public int ManufactureYear => VehicleAd.ManufactureYear;
        public string? Thumbnail => VehicleAd.VehicleImages.FirstOrDefault()?.ImagePath;
        public FuelType FuelType => VehicleAd.FuelType;
        public bool Installment => VehicleAd.Installment;
        public decimal Price => VehicleAd.Price;
        public Category Category => VehicleAd.Category;
        public string Slug => VehicleAd.Slug;
        public int ViewsCount => VehicleAd.ViewsCount;
        public int FavoritesCount => VehicleAd.FavoritesCount;
        public DateTime PostDate => VehicleAd.PublishedAt??DateTime.UtcNow;
        public bool Featured => VehicleAd.Featured && VehicleAd.FeaturedUntil>DateTime.UtcNow;
    }
}
