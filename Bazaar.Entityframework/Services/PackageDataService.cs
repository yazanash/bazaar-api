using Azure;
using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class PackageDataService(AppDbContext appDbContext) : IPackageDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<Package> CreateAsync(Package entity)
        {
            EntityEntry<Package> CreatedResult = await _appDbContext.Set<Package>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var deletedCount = await _appDbContext.Set<Package>()
                                    .Where(ad => ad.Id == id)
                                    .ExecuteDeleteAsync();

            if (deletedCount == 0) throw new ResourceNotFoundException(id, "package not found to delete");

            return deletedCount > 0;
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            IEnumerable<Package> packages = await _appDbContext.Set<Package>().AsNoTracking().ToListAsync();
            return packages;
        }

        public async Task<Package> GetByIdAsync(int id)
        {
            Package? ad = await _appDbContext.Set<Package>().FirstOrDefaultAsync(x => x.Id == id);
            if (ad == null) throw new ResourceNotFoundException(id, $"ad with ID {id} was not found.");
            return ad;
        }

        public async Task<Package> UpdateAsync(Package entity)
        {
            var exists = await _appDbContext.Set<Package>().FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (exists == null) throw new ResourceNotFoundException(entity.Id, "Ad not found to update");

            exists.MergeWith(entity);
            _appDbContext.Set<Package>().Update(exists);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
