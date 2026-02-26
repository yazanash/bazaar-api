using Bazaar.Entityframework.Models.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IStatsDataService
    {
       public Task<DashboardStats> GetStatsAsync();
    }
}
