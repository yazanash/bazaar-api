using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.CityDtos
{
    public class CityRequest
    {
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }

        public City ToModel()
        {
            return new City
            {
                ArabicName = ArabicName,
                EnglishName = EnglishName
            };
        }
    }
}
