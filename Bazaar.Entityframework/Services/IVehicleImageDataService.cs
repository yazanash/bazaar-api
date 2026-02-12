using Bazaar.Entityframework.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public interface IVehicleImageDataService
    {
        Task CreateOrUpdateRangeAsync(int vehicleId, List<VehicleImage> vehicleImage);
        Task<VehicleImage> GetAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
