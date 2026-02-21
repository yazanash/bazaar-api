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
    public class ManufacturerDataService(AppDbContext appDbContext) : IDataService<Manufacturer>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<Manufacturer> CreateAsync(Manufacturer entity)
        {
            EntityEntry<Manufacturer> CreatedResult = await _appDbContext.Set<Manufacturer>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<IEnumerable<Manufacturer>> GetAllAsync()
        {
            IEnumerable<Manufacturer> manufacturers = await _appDbContext.Set<Manufacturer>()
                .Include(m => m.VehicleModels)
        .Select(m => new Manufacturer
        {
            Id = m.Id,
            Name = m.Name,
            SupportedCategories = m.VehicleModels
                .Select(vm => vm.Category)
                .Distinct()
                .ToList()
        })
        .AsNoTracking().ToListAsync();
            return manufacturers;
        }

        public async Task<Manufacturer> GetByIdAsync(int id)
        {
            Manufacturer? manufacturer = await _appDbContext.Set<Manufacturer>().FindAsync(id);
            if (manufacturer == null) throw new ResourceNotFoundException(id, $"Manufacturer with ID {id} was not found.");
            return manufacturer;
        }

        public async Task<Manufacturer> UpdateAsync(Manufacturer entity)
        {
            _appDbContext.Set<Manufacturer>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
