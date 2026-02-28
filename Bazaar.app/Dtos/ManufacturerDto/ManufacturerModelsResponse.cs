using Bazaar.app.Dtos.VehicleModelDto;
using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.ManufacturerDto
{
    public class ManufacturerModelsResponse
    {
        private Manufacturer Manufacturer;

        public ManufacturerModelsResponse(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
        }

        public int Id => Manufacturer.Id;
        public string Name => Manufacturer.Name;
        public List<VehicleModelResponse> Models => Manufacturer.VehicleModels.Select(x=>new VehicleModelResponse(x)).ToList();
    }
}
