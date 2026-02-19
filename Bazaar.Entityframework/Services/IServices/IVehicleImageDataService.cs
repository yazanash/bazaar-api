using Bazaar.Entityframework.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IVehicleImageDataService
    {
        Task<List<string>> SyncImagesAndGetDeletablesAsync(int vehicleId, List<VehicleImage> vehicleImage);
        Task<VehicleImage> GetAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
