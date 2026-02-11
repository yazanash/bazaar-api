using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Filters
{
    public class TruckSpecsFilter : ISpecsFilter
    {
        public TruckBodyType? TruckBodyType { get; set; }
        public TrucksUsageType? TrucksUsageType { get; set; }
        public double? PayloadFrom { get; set; }
        public double? PayloadTo { get; set; }
    }
}
