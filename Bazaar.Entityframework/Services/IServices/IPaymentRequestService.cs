using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IPaymentRequestService
    {
        Task<PaymentRequest> CreateRequestAsync(PaymentRequest request);
        Task<bool> UpdateStatusAsync(int requestId, PaymentStatus newStatus, string? note);
        Task<IEnumerable<PaymentRequest>> GetPendingRequestsAsync();
    }
}
