
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;

namespace Bazaar.app.Dtos.SpecsDtos
{
    public class TruckSpecsRequest
    {
        public int AxisCount { get; set; }
        public double BackstorageLength { get; set; }
        public double BackstorageHeight { get; set; }
        public TruckBodyType TruckBodyType { get; set; }
        public TrucksUsageType TrucksUsageType { get; set; }
        public bool IsRegistered { get; set; }
        public double Payload { get; set; }
        internal TruckSpecs ToModel()
        {
            return new TruckSpecs
            {
                AxisCount = AxisCount,
                BackstorageHeight = BackstorageHeight,
                BackstorageLength = BackstorageLength,
                IsRegistered = IsRegistered,
                Payload = Payload,
                TruckBodyType = TruckBodyType,
                TrucksUsageType = TrucksUsageType,

            };
        }
    }
}
