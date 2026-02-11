using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
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
    public class UserAdDataService(AppDbContext appDbContext) : IUserAdDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<VehicleAd> CreateAsync(VehicleAd entity)
        {
            EntityEntry<VehicleAd> CreatedResult = await _appDbContext.Set<VehicleAd>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedCount = await _appDbContext.VehicleAds
                                    .Where(ad => ad.Id == id)
                                    .ExecuteDeleteAsync();

            if (deletedCount == 0) throw new ResourceNotFoundException(id, "Ad not found to delete");

            return deletedCount > 0;
        }

        public async Task<VehicleAd> GetByIdAsync(int id)
        {
            VehicleAd? ad = await _appDbContext.Set<VehicleAd>()
                .Include(x => x.VehicleModel).ThenInclude(x => x!.Manufacturer)
                .Include(x => x.City)
                .Include(x => x.TruckSpecs)
                .Include(x => x.CarSpecs)
                .Include(x => x.MotorSpecs)
                .Include(x => x.User).ThenInclude(x=>x!.Profile)
                .Include(x => x.VehicleImages)
                .Include(x => x.UserFavorites).AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id == id);
            if (ad == null) throw new ResourceNotFoundException(id, $"ad with ID {id} was not found.");
            return ad;
        }

        public async Task<PagedList<VehicleAd>> GetMyFavorites(string userId, int page, int size, string? currentUserId)
        {
            var query = _appDbContext.UserFavorites
                        .Where(f => f.UserId == userId)
                        .OrderByDescending(f => f.CreatedAt)
                        .Select(f => f.VehicleAd!) 
                      .Include(x => x.VehicleModel).ThenInclude(x => x!.Manufacturer)
                        .Include(ad => ad.City)
                        .Include(ad => ad.UserFavorites.Where(f => f.UserId == currentUserId))
                        .AsNoTracking();
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            return new PagedList<VehicleAd>(items, count, page, size);
        }

        public async Task<PagedList<VehicleAd>> GetUserAdsAsync(string userId, int page, int size)
        {
            var query = _appDbContext.VehicleAds
                     .Where(ad => ad.UserId == userId)
                     .OrderByDescending(ad => ad.PostDate)
                    .Include(x => x.VehicleModel).ThenInclude(x => x!.Manufacturer)
                     .Include(ad => ad.City)
                     .AsNoTracking();

            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return new PagedList<VehicleAd>(items, count, page, size);
        }

        public async Task<VehicleAd> UpdateAsync(VehicleAd entity)
        {
            var exists = await _appDbContext.VehicleAds.AnyAsync(x => x.Id == entity.Id);
            if (!exists) throw new ResourceNotFoundException(entity.Id, "Ad not found to update");
            _appDbContext.VehicleAds.Update(entity);
            entity.PublishStatus = PubStatus.Pending;
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
