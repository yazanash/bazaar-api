using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models.Vehicles
{
    public class UserFavorite
    {
        public string UserId { get; set; } = string.Empty;
        public int VehicleAdId { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser? User { get; set; }
        public VehicleAd? VehicleAd { get; set; }
    }
}
