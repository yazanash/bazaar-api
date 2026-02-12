using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class VehicleImageDataService(AppDbContext appDbContext) : IVehicleImageDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task CreateOrUpdateRangeAsync(int vehicleId , List<VehicleImage> incomingImages)
        {
            var existingImages = await _appDbContext.Set<VehicleImage>()
                                     .Where(x => x.VehicleId == vehicleId)
                                     .ToListAsync();

            foreach (var incoming in incomingImages)
            {
               
                var existing = existingImages.FirstOrDefault(x => x.Id == incoming.Id && incoming.Id != 0);

                if (existing != null)
                {
                    
                    existing.Order = incoming.Order;
                    _appDbContext.Update(existing);
                }
                else
                {
                    incoming.VehicleId = vehicleId;
                    await _appDbContext.Set<VehicleImage>().AddAsync(incoming);
                }
            }

            await _appDbContext.SaveChangesAsync();

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedCount = await _appDbContext.Set<VehicleImage>()
                                    .Where(i => i.Id == id)
                                    .ExecuteDeleteAsync();

            if (deletedCount == 0) throw new ResourceNotFoundException(id, "image not found to delete");

            return deletedCount > 0;
        }

        public async Task<VehicleImage> GetAsync(int id)
        {
            VehicleImage? ad = await _appDbContext.Set<VehicleImage>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ad == null) throw new ResourceNotFoundException(id, $"ad with ID {id} was not found.");
            return ad;
        }
    }
}
