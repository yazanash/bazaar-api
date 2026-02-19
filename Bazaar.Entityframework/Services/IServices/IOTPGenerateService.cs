using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IOTPGenerateService
    {
        Task<OTPModel> GenerateAsync(string email);
        Task<OTPModel?> VerifyAsync(string email, int otp);
    }
}
