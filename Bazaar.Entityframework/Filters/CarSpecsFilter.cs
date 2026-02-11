using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Filters
{
    public class CarSpecsFilter: ISpecsFilter
    {
        public UsageType? UsageType { get; set; }
        public Transmission? Transmission { get; set; }
        public CarBodyType? CarBodyType { get; set; }
        public DriveSystem? DriveSystem { get; set; }
        public bool? IsModified { get; set; }
        public int? SeatsCount { get; set; }
        public int? DoorsCount { get; set; }
    }
}
