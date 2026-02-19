using Bazaar.Entityframework.Models.UserWallet;

namespace Bazaar.app.Dtos.PackageDtos
{
    public class UserWalletResponse
    {
        private readonly UserWallet UserWallet;

        public UserWalletResponse(UserWallet userWallet)
        {
            UserWallet = userWallet;
        }
        public int AdsLimit => UserWallet.AdsLimit;
        public int FeatureLimits => UserWallet.FeatureLimits;
        public DateTime ExpiryDate => UserWallet.ExpiryDate;
    }
}
