using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Filters
{
    public class GeneralFilter
    {
        public string? Keyword { get; set; }
        public int? CityId { get; set; }
        public int? VehicleModelId { get; set; }
        public int? ManufacturerId { get; set; }
        public bool? IsUsed { get; set; }
        public FuelType? FuelType { get; set; }
        public bool? Installment { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public PostDateFilter PostDate { get; set; } = PostDateFilter.AnyTime;
        public Category? Category { get; set; }
    }
}
