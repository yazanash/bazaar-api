using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.CityDtos
{
    public class CityResponse
    {
        private City City;

        public CityResponse(City city)
        {
            City = city;
        }

        public int Id =>City.Id;
        public string? ArabicName => City.ArabicName;
        public string? EnglishName => City.EnglishName;
    }
}
