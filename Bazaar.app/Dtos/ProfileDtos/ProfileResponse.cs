using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.ProfileDtos
{
    public class ProfileResponse
    {
        private Profile Profile;

        public ProfileResponse(Profile profile)
        {
            Profile = profile;
        }
        public int Id => Profile.Id;
        public string Name => Profile.Name;
        public string PhoneNumber => Profile.PhoneNumber;
        public GenderType Gender => Profile.Gender;
        public SellerType SellerType => Profile.SellerType;
        public DateTime BirthDate => Profile.BirthDate;
    }
}
