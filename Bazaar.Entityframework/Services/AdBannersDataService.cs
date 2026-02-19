using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class AdBannersDataService(AppDbContext appDbContext) : IDataService<AdBanners>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<AdBanners> CreateAsync(AdBanners entity)
        {
            EntityEntry<AdBanners> CreatedResult = await _appDbContext.Set<AdBanners>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<IEnumerable<AdBanners>> GetAllAsync()
        {
            var now = DateTime.UtcNow;
            IEnumerable<AdBanners> banners = await _appDbContext.Set<AdBanners>()
                .Where(x => x.ExpirationDate >= now && x.ActivationDate <= now).AsNoTracking().ToListAsync();
            return banners;
        }

        public async Task<AdBanners> GetByIdAsync(int id)
        {
            AdBanners? banner = await _appDbContext.Set<AdBanners>().FindAsync(id);
            if (banner == null) throw new ResourceNotFoundException(id, $"banner with ID {id} was not found.");
            return banner;
        }

        public async Task<AdBanners> UpdateAsync(AdBanners entity)
        {
            _appDbContext.Set<AdBanners>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
