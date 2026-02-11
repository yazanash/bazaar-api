using Bazaar.Entityframework.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public interface IUserAdDataService
    {
        Task<PagedList<VehicleAd>> GetMyFavorites(string userId, int page, int size, string? currentUserId);
        Task<VehicleAd> CreateAsync(VehicleAd entity);
        Task<VehicleAd> UpdateAsync(VehicleAd entity);
        Task<bool> DeleteAsync(int id);
        Task<VehicleAd> GetByIdAsync(int id);
        Task<PagedList<VehicleAd>> GetUserAdsAsync(string userId, int page, int size);
    }
}
