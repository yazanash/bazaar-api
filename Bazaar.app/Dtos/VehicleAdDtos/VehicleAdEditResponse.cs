using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.SpecsDtos;
using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class VehicleAdEditResponse
    {
        private VehicleAd VehicleAd;

        public VehicleAdEditResponse(VehicleAd vehicleAd)
        {
            VehicleAd = vehicleAd;
        }
        public int CityId => VehicleAd.CityId;
        public int VehicleModelId => VehicleAd.VehicleModelId;

        public List<VehicleImageResponse>? Gallery => VehicleAd.VehicleImages.Select(x => new VehicleImageResponse(x)).ToList();
        public int Id => VehicleAd.Id;
        public CityResponse? City => new(VehicleAd.City ?? new City());
        public ManufacturerModelResponse? VehicleModel => new ManufacturerModelResponse(VehicleAd.VehicleModel ?? new VehicleModel());
        public int ManufactureYear => VehicleAd.ManufactureYear;
        public bool IsUsed => VehicleAd.IsUsed;
        public FuelType FuelType => VehicleAd.FuelType;
        public bool Installment => VehicleAd.Installment;
        public decimal Price => VehicleAd.Price;
        public Category Category => VehicleAd.Category;
        public double UsedKilometers => VehicleAd.UsedKilometers;
        public string Color => VehicleAd.Color;
        public string Description => VehicleAd.Description;
        public CarSpecsResponse? CarSpecs => Category == Category.Passenger && VehicleAd.CarSpecs != null ? new CarSpecsResponse(VehicleAd.CarSpecs) : null;
        public TruckSpecsResponse? TruckSpecs => Category == Category.Trucks && VehicleAd.TruckSpecs != null ? new TruckSpecsResponse(VehicleAd.TruckSpecs) : null;
        public MotorSpecsResponse? MotorSpecs => Category == Category.Motorcycles && VehicleAd.MotorSpecs != null ? new MotorSpecsResponse(VehicleAd.MotorSpecs) : null;
    }
}
