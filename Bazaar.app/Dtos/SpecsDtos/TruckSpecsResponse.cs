using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.SpecsDtos
{
    public class TruckSpecsResponse
    {
        private TruckSpecs TruckSpecs;

        public TruckSpecsResponse(TruckSpecs truckSpecs)
        {
            TruckSpecs = truckSpecs;
        }
        public int AxisCount => TruckSpecs.AxisCount;
        public double BackstorageLenght => TruckSpecs.BackstorageLenght;
        public double BackstorageHeight => TruckSpecs.BackstorageHeight;
        public TruckBodyType TruckBodyType => TruckSpecs.TruckBodyType;
        public TrucksUsageType TrucksUsageType => TruckSpecs.TrucksUsageType;
        public bool IsRegistered => TruckSpecs.IsRegistered;
        public double Payload => TruckSpecs.Payload;
    }
}
