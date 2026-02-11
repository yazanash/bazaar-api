using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.VehicleModelDto
{
    public class VehicleModelRequest
    {
        public string Name { get; set; } = string.Empty;
        public int ManufacturerId { get; set; }
        public Category Category { get; set; }

        public VehicleModel ToModel()
        {
            return new VehicleModel { Name = Name, ManufacturerId = ManufacturerId, Category = Category };
        }
    }
}
