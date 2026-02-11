using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.VehicleModelDto
{
    public class VehicleModelResponse
    {
        private VehicleModel VehicleModel;

        public VehicleModelResponse(VehicleModel vehicleModel)
        {
            VehicleModel = vehicleModel;
        }
        public int Id => VehicleModel.Id;
        public string Name => VehicleModel.Name;
        public int ManufacturerId => VehicleModel.ManufacturerId;
        public Category Category => VehicleModel.Category;
    }
}
