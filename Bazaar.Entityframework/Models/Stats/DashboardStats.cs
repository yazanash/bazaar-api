using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Stats
{
    public class DashboardStats
    {
        public Summary Summary { get; set; } = new Summary();
        public List<GrowthData> GrowthData { get; set; } = new List<GrowthData>();
        public List<StatusData> StatusData { get; set; } = new List<StatusData>();
    }

}
