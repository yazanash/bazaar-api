using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Filters
{
    public class MotorSpecsFilter : ISpecsFilter
    {
        public MotorTransmission? MotorTransmission { get; set; }
        public MotorBodyType? MotorBodyType { get; set; }
        public bool? IsModified { get; set; }
    }
}
