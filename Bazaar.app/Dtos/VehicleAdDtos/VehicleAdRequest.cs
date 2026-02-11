using Bazaar.app.Dtos.SpecsDtos;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class VehicleAdRequest
    {
        public int CityId { get; set; }
        public int VehicleModelId { get; set; }
        public int ManufactureYear { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public bool IsUsed { get; set; }
        public FuelType FuelType { get; set; }
        public bool Installment { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public double UsedKilometers { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public CarSpecsRequest? CarSpecs { get; set; }
        public TruckSpecsRequest? TruckSpecs { get; set; }
        public MotorSpecsRequest? MotorSpecs { get; set; }
        public List<IFormFile>? Gallery { get; set; }
        public VehicleAd ToModel()
        {
            VehicleAd vehicleAd=  new()
            {
                Category = Category,
                CityId = CityId,
                Color = Color,
                IsUsed = IsUsed,
                Description = Description,
                FuelType = FuelType,
                Installment = Installment,
                ManufactureYear = ManufactureYear,
                Price = Price,
                UsedKilometers = UsedKilometers,
                VehicleModelId = VehicleModelId,
                CarSpecs = CarSpecs?.ToModel() ?? null,
                TruckSpecs = TruckSpecs?.ToModel() ?? null,
                MotorSpecs = MotorSpecs?.ToModel() ?? null,
            };
            vehicleAd.CarSpecs = (Category == Category.Passenger) ? CarSpecs?.ToModel() : null;
            vehicleAd.TruckSpecs = (Category == Category.Trucks) ? TruckSpecs?.ToModel() : null;
            vehicleAd.MotorSpecs = (Category == Category.Motorcycles) ? MotorSpecs?.ToModel() : null;
            return vehicleAd;   
        }
    }

}
