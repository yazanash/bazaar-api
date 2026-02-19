using Bazaar.Entityframework.Filters;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IAdDataService
    {
        Task<PagedList<VehicleAd>> GetAllStarredAsync(int page, int size, string? currentUserId);
        Task<PagedList<VehicleAd>> GetByPostStatusAsync(int page, int size, string? currentUserId,PubStatus pubStatus );
        Task<PagedList<VehicleAd>> SearchAsync(GeneralFilter generalFilter,ISpecsFilter? specsFilter, int page, int size, string? currentUserId);
        Task<VehicleAd> GetBySlugAsync(string slug, string? currentUserId);
        Task<bool> ToggleVehicleFavoriteAsync(int id,string userId );
        Task<bool> ChangeAddStatus(int vehicleAdId,PubStatus pubStatus,string? reason);
        Task<PagedList<VehicleAd>> GetUserFavoritesAsync(int page, int size,string currentUserId);
    }
}
