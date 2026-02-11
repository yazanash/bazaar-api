
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.SpecsDtos
{
    public class CarSpecsRequest
    {
        public Transmission Transmission { get; set; }
        public RegistrationType RegistrationType { get; set; }
        public CarBodyType CarBodyType { get; set; }
        public DriveSystem DriveSystem { get; set; }
        public bool IsModified { get; set; }
        public string ModificationDescription { get; set; } = string.Empty;
        public int SeatsCount { get; set; }
        public int DoorsCount { get; set; }
        public UsageType UsageType { get; set; }
        public CarSpecs ToModel()
        {
            return new CarSpecs
            {
                Transmission = Transmission,
                RegistrationType = RegistrationType,
                CarBodyType = CarBodyType,
                DriveSystem = DriveSystem,
                IsModified = IsModified,
                ModificationDescription = ModificationDescription,
                SeatsCount = SeatsCount,
                DoorsCount = DoorsCount,
                UsageType = UsageType,
            };

        }
    }
}
