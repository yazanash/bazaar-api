using Bazaar.app.Dtos.CityDtos;
using Bazaar.app.Dtos.VehicleModelDto;

namespace Bazaar.app.Dtos
{
    public class MasterResponse
    {
        public IEnumerable<CityResponse> Cities { get; set; }=new List<CityResponse>();
        public IEnumerable<ManufacturerModelResponse> Models { get; set; } = new List<ManufacturerModelResponse>();

    }
}
