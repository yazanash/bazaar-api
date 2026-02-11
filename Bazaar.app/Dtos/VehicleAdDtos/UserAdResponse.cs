using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class UserAdResponse
    {
        public PubStatus PubStatus { get; set; }
        public string? Reasone { get; set; }

        public VehicleAdResponse? VehicleAdResponse { get; set; }

    }
}
