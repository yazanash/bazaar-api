using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.ManufacturerDto
{
    public class ManufacturerResponse
    {
        private Manufacturer Manufacturer;

        public ManufacturerResponse(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
        }

        public int Id => Manufacturer.Id;
        public string Name => Manufacturer.Name;
        public List<Category> SupportedCategries => Manufacturer.SupportedCategories;
    }
}
