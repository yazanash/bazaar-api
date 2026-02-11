using Bazaar.app.Dtos;
using Bazaar.Entityframework.Filters;
using Bazaar.Entityframework.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bazaar.app.Helpers
{
    public class SearchRequestBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = new SearchRequest();
            var query = bindingContext.ValueProvider;

            request.General.Keyword = query.GetValue("Keyword").FirstValue;
            request.General.Category = TryGetEnum<Category>(query, "Category");
            request.General.PriceTo = TryGetDecimal(query, "PriceTo");
            request.General.CityId = TryGetInt(query, "CityId");
            request.General.VehicleModelId = TryGetInt(query, "VehicleModelId");
            request.General.IsUsed = TryGetBool(query, "IsUsed");
            request.General.FuelType = TryGetEnum<FuelType>(query, "FuelType");
            request.General.Installment = TryGetBool(query, "Installment");
            request.General.PriceFrom = TryGetInt(query, "PriceFrom");
            request.General.PostDate = TryGetEnum<PostDateFilter>(query, "PostDate")?? PostDateFilter.AnyTime;
            if (request.General.Category.HasValue)
            {
                request.Specs = request.General.Category.Value switch
                {
                    Category.Passenger => BindSpecs<CarSpecsFilter>(query),
                    Category.Trucks => BindSpecs<TruckSpecsFilter>(query),
                    Category.Motorcycles => BindSpecs<MotorSpecsFilter>(query),
                    _ => null
                };
            }

            bindingContext.Result = ModelBindingResult.Success(request);
            return Task.CompletedTask;
        }
        private T BindSpecs<T>(IValueProvider query) where T : class, new()
        {
            var specs = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                var value = query.GetValue(prop.Name).FirstValue;
                if (string.IsNullOrEmpty(value)) continue;

                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (targetType.IsEnum)
                {
                    if (Enum.TryParse(targetType, value, true, out var enumResult))
                        prop.SetValue(specs, enumResult);
                }
                else
                {
                    prop.SetValue(specs, Convert.ChangeType(value, targetType));
                }
            }
            return specs;
        }
        private int? TryGetInt(IValueProvider q, string key) => int.TryParse(q.GetValue(key).FirstValue, out var v) ? v : null;
        private decimal? TryGetDecimal(IValueProvider q, string key) => decimal.TryParse(q.GetValue(key).FirstValue, out var v) ? v : null;

        private bool? TryGetBool(IValueProvider q, string key) => bool.TryParse(q.GetValue(key).FirstValue, out var v) ? v : null;
        private TEnum? TryGetEnum<TEnum>(IValueProvider query, string key) where TEnum : struct
        {
            var value = query.GetValue(key).FirstValue;
            if (!string.IsNullOrEmpty(value) && Enum.TryParse<TEnum>(value, true, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
