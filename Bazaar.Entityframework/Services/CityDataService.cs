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
    public class CityDataService(AppDbContext appDbContext) : IDataService<City>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<City> CreateAsync(City entity)
        {
            EntityEntry<City> CreatedResult = await _appDbContext.Set<City>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            IEnumerable<City> cities = await _appDbContext.Set<City>().AsNoTracking().ToListAsync();
            return cities;
        }

        public async Task<City> GetByIdAsync(int id)
        {
            City? city = await _appDbContext.Set<City>().FindAsync(id);
            if (city == null) throw new ResourceNotFoundException(id, $"City with ID {id} was not found.");
            return city;
        }

        public async Task<City> UpdateAsync(City entity)
        {
            _appDbContext.Set<City>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
