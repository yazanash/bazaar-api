using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class VehicleModelDataService(AppDbContext appDbContext) : IDataService<VehicleModel>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<VehicleModel> CreateAsync(VehicleModel entity)
        {
            EntityEntry<VehicleModel> CreatedResult = await _appDbContext.Set<VehicleModel>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<IEnumerable<VehicleModel>> GetAllAsync()
        {
            IEnumerable<VehicleModel> models = await _appDbContext.Set<VehicleModel>().Include(x=>x.Manufacturer).AsNoTracking().ToListAsync();
            return models;
        }

        public async Task<VehicleModel> GetByIdAsync(int id)
        {
            VehicleModel? model = await _appDbContext.Set<VehicleModel>().FindAsync(id);
            if (model == null) throw new ResourceNotFoundException(id, $"model with ID {id} was not found.");
            return model;
        }

        public async Task<VehicleModel> UpdateAsync(VehicleModel entity)
        {
            _appDbContext.Set<VehicleModel>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
