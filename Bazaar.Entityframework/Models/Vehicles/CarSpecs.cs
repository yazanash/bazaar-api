using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Vehicles
{
    public class CarSpecs
    {
        [Key, ForeignKey("VehicleAd")]
        public int VehicleId { get; set; }
        public virtual VehicleAd? VehicleAd { get; set; }
        public Transmission Transmission { get; set; }
        public RegistrationType RegistrationType { get; set; }
        public CarBodyType CarBodyType { get; set; }
        public DriveSystem DriveSystem { get; set; }
        public bool IsModified { get; set; }
        public string ModificationDescription { get; set; } = string.Empty;
        public int SeatsCount{ get; set; }
        public int DoorsCount { get; set; }
        public UsageType UsageType { get; set; }
    }
}
