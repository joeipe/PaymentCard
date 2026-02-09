using Microsoft.Extensions.Configuration;

namespace PaymentCard.API.Configurations
{
    public static class ApplicationInsightsConfig
    {
        public static void AddApplicationInsightsConfiguration(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            //  App Service → Configuration → Application settings = APPLICATIONINSIGHTS_CONNECTION_STRING
            if (!environment.IsDevelopment())
            {
                services.AddApplicationInsightsTelemetry();
            }
        }
    }
}
