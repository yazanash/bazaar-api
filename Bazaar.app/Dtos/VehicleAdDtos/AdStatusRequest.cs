using Bazaar.Entityframework.Models;

namespace Bazaar.app.Dtos.VehicleAdDtos
{
    public class AdStatusRequest
    {
       public PubStatus PubStatus { get; set; }
       public string? Reasone { get; set; }
    }
}
