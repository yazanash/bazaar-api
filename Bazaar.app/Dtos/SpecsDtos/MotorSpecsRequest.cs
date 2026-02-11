
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.SpecsDtos
{
    public class MotorSpecsRequest
    {
        public MotorTransmission MotorTransmission { get; set; }
        public bool IsRegistered { get; set; }
        public MotorBodyType MotorBodyType { get; set; }
        public bool IsModified { get; set; }
        public string ModificationDescription { get; set; } = string.Empty;
        internal MotorSpecs ToModel()
        {
            return new MotorSpecs
            {
                MotorTransmission= MotorTransmission,
                IsRegistered = IsRegistered,
                MotorBodyType = MotorBodyType,
                IsModified = IsModified,
                ModificationDescription = ModificationDescription,
            };
        }
    }
}
