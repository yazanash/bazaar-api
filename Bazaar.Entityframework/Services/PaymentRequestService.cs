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
    public class PaymentRequestService(AppDbContext context) : IPaymentRequestService
    {
        private readonly AppDbContext _context = context;
        public async Task<PaymentRequest> CreateRequestAsync(PaymentRequest request)
        {
            EntityEntry<PaymentRequest> CreatedResult = await _context.Set<PaymentRequest>().AddAsync(request);
            await _context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<IEnumerable<PaymentRequest>> GetPendingRequestsAsync()
        {
            IEnumerable<PaymentRequest> requests = await _context.Set<PaymentRequest>().Where(x=>x.Status == PaymentStatus.Pending).AsNoTracking().ToListAsync();
            return requests;
        }

        public async Task<bool> UpdateStatusAsync(int requestId, PaymentStatus newStatus, string? note)
        {
            var rowsAffected = await _context.Set<PaymentRequest>()
        .Where(x => x.Id == requestId)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(v => v.Status, newStatus).SetProperty(v => v.AdminNote, note));

            if (rowsAffected == 0) throw new ResourceNotFoundException(requestId, $"request with ID {requestId} was not found.");
            return true;
        }
    }
}
