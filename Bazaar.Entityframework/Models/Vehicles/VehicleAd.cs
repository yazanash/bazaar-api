using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Vehicles
{
    public class VehicleAd
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual AppUser? User { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int CityId { get; set; }
        public City? City { get; set; }
        public int VehicleModelId { get; set; }
        public VehicleModel? VehicleModel { get; set; }
        public int ManufactureYear { get; set; }
        public bool IsUsed { get; set; }
        public FuelType FuelType { get; set; }
        public bool Installment { get; set; }
        public DateTime PostDate { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public double UsedKilometers { get; set; }
        public string Color { get; set; } = string.Empty;
        public virtual CarSpecs? CarSpecs { get; set; }
        public virtual TruckSpecs? TruckSpecs { get; set; }
        public virtual MotorSpecs? MotorSpecs { get; set; }
        public virtual List<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();
        public PubStatus PublishStatus { get; set; } = PubStatus.Pending;
        public string? RejectionReason { get; set; }
        public DateTime? PublishedAt { get; set; }
        public int ViewsCount { get; set; }
        public int FavoritesCount { get; set; }
        public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
        public bool Special { get; set; }
        public void GenerateSlug(string? manufacturerName, string? modelName, string? cityEnglishName, int manufactureYear)
        {

            string timestamp = DateTime.UtcNow.Ticks.ToString().Substring(10);
            string manufacturer = (manufacturerName ?? "vehicle").Replace(" ", "-").ToLower();
            string model = (modelName ?? "type").Replace(" ", "-").ToLower();
            string city = (cityEnglishName ?? "City").Replace(" ", "-").ToLower();
            Slug = $"{manufacturer}-{model}-{manufactureYear}-{city}-{timestamp}";
        }
        public void MergeWith(VehicleAd vehicleAd)
        {
            Description = vehicleAd.Description;
            CityId = vehicleAd.CityId;
            VehicleModelId = vehicleAd.VehicleModelId;
            ManufactureYear = vehicleAd.ManufactureYear;
            IsUsed = vehicleAd.IsUsed;
            FuelType = vehicleAd.FuelType;
            Installment = vehicleAd.Installment;
            PostDate = vehicleAd.PostDate;
            Price = vehicleAd.Price;
            Category = vehicleAd.Category;
            UsedKilometers = vehicleAd.UsedKilometers;
            Color = vehicleAd.Color;
            CarSpecs = SyncSpecs(CarSpecs, vehicleAd.CarSpecs);
            TruckSpecs = SyncSpecs(TruckSpecs, vehicleAd.TruckSpecs);
            MotorSpecs = SyncSpecs(MotorSpecs, vehicleAd.MotorSpecs);

        }
        private T? SyncSpecs<T>(T? existing, T? incoming) where T : class
        {
            if (existing != null && incoming != null)
            {
               
                (existing as dynamic).MergeWith(incoming as dynamic);
                return existing;
            }
            return incoming;
        }
    }
}
