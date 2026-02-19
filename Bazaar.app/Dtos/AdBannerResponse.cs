using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos
{
    public class AdBannerResponse
    {
        private AdBanners  AdBanners;

        public AdBannerResponse(AdBanners adBanners)
        {
            AdBanners = adBanners;
        }
        public int Id => AdBanners.Id;
        public string ImageUrl => AdBanners.ImageUrl;
        public string Link => AdBanners.Link;
        public DateTime ExpirationDate => AdBanners.ExpirationDate;
        public DateTime ActivationDate => AdBanners.ActivationDate;
    }
}
