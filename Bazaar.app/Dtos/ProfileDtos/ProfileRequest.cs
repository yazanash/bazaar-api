using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.ProfileDtos
{
    public class ProfileRequest
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public SellerType SellerType { get; set; }
        public DateTime BirthDate { get; set; }
        public Profile ToModel()
        {
            return new Profile
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                Gender = Gender,
                SellerType = SellerType,
                 BirthDate = BirthDate
            };

        }
      
    }
}
