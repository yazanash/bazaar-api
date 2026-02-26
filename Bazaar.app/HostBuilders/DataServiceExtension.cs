using Bazaar.app.Services;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services;
using Bazaar.Entityframework.Services.IServices;

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
            services.AddScoped<IVehicleImageDataService, VehicleImageDataService>();
            services.AddScoped<IUserWalletService, UserWalletDataService>();
            services.AddScoped<IPackageDataService, PackageDataService>();
            services.AddScoped<IDataService<AdBanners>, AdBannersDataService>();
            services.AddScoped<IStatsDataService, StatsDataService>();
            services.AddScoped<IPaymentGatewayDataService, PaymentGatewayDataService>();
            services.AddScoped<IUserStateService, UserStateService>();
            services.AddScoped<IPaymentRequestService, PaymentRequestService>();
            return services;
        }

    }
}
