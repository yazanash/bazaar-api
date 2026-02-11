using Bazaar.app.Services;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services;

namespace Bazaar.app.HostBuilders
{
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IAdDataService,AdDataService>();
            services.AddScoped<IOTPGenerateService, OTPGenerateService>();
            services.AddScoped<IProfileDataService, ProfileDataService>();
            services.AddScoped<IUserAdDataService, UserAdDataService>();
            services.AddScoped<IDataService<Manufacturer>, ManufacturerDataService>();
            services.AddScoped<IDataService<VehicleModel>, VehicleModelDataService>();
            services.AddScoped<IDataService<City>, CityDataService>();
            return services;
        }

    }
}
