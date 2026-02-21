using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.ManufacturerDto
{
    public class ManufacturerRequest
    {
        public string Name { get; set; } = string.Empty;

        public Manufacturer ToModel()
        {
            return new Manufacturer { Name = Name  };
        }
    }
}
