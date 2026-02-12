using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Vehicles
{
    public class MotorSpecs
    {
        [Key, ForeignKey("VehicleAd")] 
        public int VehicleId { get; set; }
        public virtual VehicleAd? VehicleAd { get; set; }
        public MotorTransmission MotorTransmission{ get; set; }
        public bool IsRegistered{get;set; }
        public MotorBodyType MotorBodyType { get; set; }
        public bool IsModified { get; set; }
        public string ModificationDescription { get; set; } = string.Empty;

        public void MergeWith(MotorSpecs motorSpecs)
        {
            MotorTransmission = motorSpecs.MotorTransmission;
            IsRegistered = motorSpecs.IsRegistered;
            MotorBodyType = motorSpecs.MotorBodyType;
            IsModified = motorSpecs.IsModified;
            ModificationDescription = motorSpecs.ModificationDescription;
        }
    }
}
