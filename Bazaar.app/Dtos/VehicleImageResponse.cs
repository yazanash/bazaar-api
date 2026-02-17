using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos
{
    public class VehicleImageResponse
    {
        private VehicleImage VehicleImage;

        public VehicleImageResponse(VehicleImage vehicleImage)
        {
            VehicleImage = vehicleImage;
        }

        public int Id => VehicleImage.Id;
        public string ImageUrl =>  VehicleImage.ImagePath;
        public int Order => VehicleImage.Order;
    }
}
