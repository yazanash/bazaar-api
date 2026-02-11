using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class OTPGenerateService(AppDbContext appDbContext) : IOTPGenerateService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<OTPModel> GenerateAsync(string email)
        {
            List<OTPModel> otps = await _appDbContext.Set<OTPModel>().Where(x => x.Email == email).ToListAsync();
            _appDbContext.RemoveRange(otps);
            OTPModel model = new OTPModel();
            model.Email = email;
            model.Otp = new Random().Next(111111, 999999);
            model.ExpireDate = DateTime.Now.AddMinutes(30);
            await _appDbContext.AddAsync(model);
            await _appDbContext.SaveChangesAsync();
            return model;
        }

        public async Task<OTPModel?> VerifyAsync(string email, int otp)
        {
            OTPModel? oTPModel = await _appDbContext.Set<OTPModel>().FirstOrDefaultAsync(x => x.Email == email);
            if (oTPModel != null && oTPModel.Otp == otp && oTPModel.ExpireDate.Subtract(DateTime.Now).TotalMinutes > 0)
            {
                _appDbContext.Set<OTPModel>().Remove(oTPModel);
                await _appDbContext.SaveChangesAsync();
                return oTPModel;
            }
            return null;
        }
    }
}
