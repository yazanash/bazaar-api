using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.SpecsDtos
{
    public class MotorSpecsResponse
    {
        private MotorSpecs MotorSpecs;

        public MotorSpecsResponse(MotorSpecs motorSpecs)
        {
            MotorSpecs = motorSpecs;
        }
        public MotorTransmission MotorTransmission => MotorSpecs.MotorTransmission;
        public bool IsRegistered => MotorSpecs.IsRegistered;
        public MotorBodyType MotorBodyType => MotorSpecs.MotorBodyType;
        public bool IsModified => MotorSpecs.IsModified;
        public string ModificationDescription => MotorSpecs.ModificationDescription;
    }
}
