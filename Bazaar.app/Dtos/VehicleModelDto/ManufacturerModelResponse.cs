using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.VehicleModelDto
{
    public class ManufacturerModelResponse
    {
        private VehicleModel VehicleModel;

        public ManufacturerModelResponse(VehicleModel vehicleModel)
        {
            VehicleModel = vehicleModel;
        }
        public int Id => VehicleModel.Id;
        public string Name => $"{VehicleModel.Manufacturer?.Name} / {VehicleModel.Name}";
        public Category Category => VehicleModel.Category;
        public int ManufacturerId => VehicleModel.ManufacturerId;
    }
}
