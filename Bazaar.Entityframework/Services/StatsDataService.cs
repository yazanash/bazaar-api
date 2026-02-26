using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Models.Stats;
using Bazaar.Entityframework.Models.UserWallet;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class StatsDataService(AppDbContext appDbContext) : IStatsDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<DashboardStats> GetStatsAsync()
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var totalAds = await _appDbContext.Set<VehicleAd>().CountAsync();
            var pendingAds = await _appDbContext.Set<VehicleAd>().CountAsync(a => a.PublishStatus == Models.PubStatus.Pending);

            var monthlyRevenue = await _appDbContext.Set<PackageBundle>()
                .Where(pb => pb.CreatedAt >= startOfMonth)
                .SumAsync(pb => pb.Price);

            var growthData = await _appDbContext.Set<VehicleAd>()
                .Where(a => a.PostDate >= now.AddMonths(-5))
                .GroupBy(a => new { a.PostDate.Year, a.PostDate.Month })
                .Select(g => new GrowthData
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                    Ads = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToListAsync();

            var statusData = await _appDbContext.Set<VehicleAd>()
                .GroupBy(a => a.PublishStatus)
                .Select(g => new StatusData
                {
                    Status = g.Key.ToString().ToLower(),
                    Count = g.Count()
                })
                .ToListAsync();

            return new DashboardStats
            {
                Summary = new Summary
                {
                    TotalAds = totalAds,
                    PendingAds = pendingAds,
                    MonthlyRevenue = monthlyRevenue
                },
                GrowthData = growthData,
                StatusData = statusData
            };
        }
    }
}
