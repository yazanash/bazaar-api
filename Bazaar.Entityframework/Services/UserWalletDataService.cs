using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.UserWallet;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Entityframework.Services
{
    public class UserWalletDataService(AppDbContext appDbContext) : IUserWalletService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<bool> ConsumeAdCredit(string userId, int adId)
        {
            if (await _appDbContext.Set<CreditTransaction>().AnyAsync(t => t.AdId == adId && t.Type == TransactionType.AdUsage))
                return true;

            var wallet = await _appDbContext.Set<UserWallet>().FirstOrDefaultAsync(w => w.UserId == userId);
            var now = DateTime.UtcNow;

            if (wallet == null || wallet.ExpiryDate < now || wallet.AdsLimit <= 0)
                return false;

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                wallet.AdsLimit--;

                _appDbContext.Set<CreditTransaction>().Add(new CreditTransaction
                {
                    AdId = adId,
                    Type = TransactionType.AdUsage,
                    UserId = userId
                });
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<bool> ConsumeFeatureCredit(string userId, int adId)
        {
            var wallet = await _appDbContext.Set<UserWallet>().FirstOrDefaultAsync(w => w.UserId == userId);
            var now = DateTime.UtcNow;

            if (wallet == null || wallet.ExpiryDate < now || wallet.FeatureLimits <= 0)
                return false;

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                wallet.FeatureLimits--;

                _appDbContext.Set<CreditTransaction>().Add(new CreditTransaction
                {
                    AdId = adId,
                    Type = TransactionType.Featured,
                    UserId = userId
                });
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<UserWallet> GetUserWallet(string userId)
        {
            var now = DateTime.UtcNow;
            var userWallet = await _appDbContext.Set<UserWallet>()
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();
            if (userWallet != null) return userWallet;

            return new UserWallet { UserId = userId, AdsLimit = 0, FeatureLimits = 0, ExpiryDate = DateTime.UtcNow };
        }
        public async Task<PackageBundle> CreatePackageBundle(string userId, int packageId)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                Package? package = await _appDbContext.Set<Package>().FirstOrDefaultAsync(x => x.Id == packageId);
                if (package == null) throw new ResourceNotFoundException($"package With id {packageId} was not found.");
                var now = DateTime.UtcNow;

                var bundle = new PackageBundle
                {
                    UserId = userId,
                    PackageId = package.Id,
                    AdsLimit = package.AdLimits,
                    PackageName = package.Name, 
                    Price = package.Price,
                    FeaturedLimit = package.FeaturedLimit
                };
                await _appDbContext.Set<PackageBundle>().AddAsync(bundle);

                var wallet = await _appDbContext.Set<UserWallet>().FirstOrDefaultAsync(w => w.UserId == userId);
                if (wallet == null)
                {
                    wallet = new UserWallet { UserId = userId, AdsLimit = package.AdLimits, FeatureLimits = package.FeaturedLimit, ExpiryDate = now.AddDays(60) };
                    await _appDbContext.Set<UserWallet>().AddAsync(wallet);
                }
                else
                {
                    if (wallet.ExpiryDate < now)
                    {
                        wallet.AdsLimit = 0;
                        wallet.FeatureLimits = 0;
                    }
                    wallet.AdsLimit += package.AdLimits;
                    wallet.FeatureLimits += package.FeaturedLimit;
                    wallet.ExpiryDate = now.AddDays(60);
                }

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return bundle;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
