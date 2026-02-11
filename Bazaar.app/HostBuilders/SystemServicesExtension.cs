using Bazaar.app.Services;
using Bazaar.app.Services.TesterServices;

namespace Bazaar.app.HostBuilders
{
    public static class SystemServicesExtension
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {
            services.AddSingleton<EmailService>();
            services.AddScoped<IJwtTokenService, TokenGenerationService>();
            //services.AddScoped<IOTPGenerateService<OTPModel>, OTPGenerateService>();
            services.AddScoped<IBypassService, BypassService>();
            services.AddScoped<WebPImageService>();


            return services;
        }
    }
}
