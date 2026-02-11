using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Vehicles
{
    public class TruckSpecs
    {
        [Key, ForeignKey("VehicleAd")]
        public int VehicleId { get; set; }
        public virtual VehicleAd? VehicleAd { get; set; }
        public int AxisCount { get; set; }
        public double BackstorageLenght { get;set; }
        public double BackstorageHeight { get; set; }
        public TruckBodyType TruckBodyType { get; set; }
        public TrucksUsageType TrucksUsageType { get; set; }
        public bool IsRegistered { get; set; }
        public double Payload{ get; set; }
    }
}
