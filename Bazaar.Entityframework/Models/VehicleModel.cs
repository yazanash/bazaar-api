using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public int ManufacturerId { get; set; }
        public Category Category { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }
    }
}
