using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.SpecsDtos
{
    public class CarSpecsResponse
    {
        private CarSpecs carSpecs;

        public CarSpecsResponse(CarSpecs carSpecs)
        {
            this.carSpecs = carSpecs;
        }
        public Transmission Transmission => carSpecs.Transmission;
        public RegistrationType RegistrationType => carSpecs.RegistrationType;
        public CarBodyType CarBodyType => carSpecs.CarBodyType;
        public DriveSystem DriveSystem => carSpecs.DriveSystem;
        public bool IsModified => carSpecs.IsModified;
        public string ModificationDescription => carSpecs.ModificationDescription;
        public int SeatsCount => carSpecs.SeatsCount;
        public int DoorsCount => carSpecs.DoorsCount;
        public UsageType UsageType => carSpecs.UsageType;
    }
}
