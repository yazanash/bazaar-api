using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Stats
{
    public class Summary
    {
        public int TotalAds { get; set; }
        public int PendingAds { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }
}
