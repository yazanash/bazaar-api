using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IPaymentGatewayDataService
    {
        Task<IEnumerable<PaymentGateway>> GetAllAsync();
        Task<PaymentGateway> CreateAsync(PaymentGateway gateway);
        Task<PaymentGateway> UpdateAsync(PaymentGateway gateway);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
        Task<PaymentGateway> GetByIdAsync(int id);
    }
}
