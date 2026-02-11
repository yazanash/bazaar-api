using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Vehicles
{
    public class VehicleImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public int VehicleId { get; set; }
    }
}
