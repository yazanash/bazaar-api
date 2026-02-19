using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos
{
    public class AdBannerRequest
    {
        public int Id { get; set; }
        public IFormFile? ImageUrl {  get; set; }
        public string Link { get; set; } = string.Empty;
        public DateTime ActivationDate { get; set; }
        public int DurationDays { get; set; }
        internal AdBanners ToModel()
        {
            return new AdBanners
            {
                Link = Link,
                ActivationDate = ActivationDate,
                ExpirationDate= ActivationDate.AddDays(DurationDays)
            };
        }
    }
}
