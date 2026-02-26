using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
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
    public class PaymentGatewayDataService(AppDbContext appDbContext) : IPaymentGatewayDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<PaymentGateway> CreateAsync(PaymentGateway gateway)
        {
            EntityEntry<PaymentGateway> CreatedResult = await _appDbContext.Set<PaymentGateway>().AddAsync(gateway);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedCount = await _appDbContext.Set<PaymentGateway>()
                                     .Where(ad => ad.Id == id)
                                     .ExecuteDeleteAsync();

            if (deletedCount == 0) throw new ResourceNotFoundException(id, "Gateway not found to delete");

            return deletedCount > 0;
        }

        public async Task<IEnumerable<PaymentGateway>> GetAllAsync()
        {
            IEnumerable<PaymentGateway> gateways = await _appDbContext.Set<PaymentGateway>().AsNoTracking().ToListAsync();
            return gateways;
        }

        public async Task<PaymentGateway> GetByIdAsync(int id)
        {
            PaymentGateway? gateway = await _appDbContext.Set<PaymentGateway>().FindAsync(id);
            if (gateway == null) throw new ResourceNotFoundException(id, $"Gateway with ID {id} was not found.");
            return gateway;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var rowsAffected = await _appDbContext.Set<PaymentGateway>()
         .Where(x => x.Id == id)
         .ExecuteUpdateAsync(setters => setters
             .SetProperty(v => v.IsActive, v => !v.IsActive));

            if (rowsAffected == 0) throw new ResourceNotFoundException(id, $"Gateway with ID {id} was not found.");
            return true;
        }

        public async Task<PaymentGateway> UpdateAsync(PaymentGateway gateway)
        {
            _appDbContext.Set<PaymentGateway>().Update(gateway);
            await _appDbContext.SaveChangesAsync();
            return gateway;
        }
    }
}
