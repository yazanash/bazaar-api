using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.UserWallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IUserWalletService
    {
        Task<UserWallet> GetUserWallet(string userId);
        Task<bool> ConsumeAdCredit(string userId, int adId);
        Task<bool> ConsumeFeatureCredit(string userId, int adId);
        Task<PackageBundle> CreatePackageBundle(string userId, int packageId);

    }
}
