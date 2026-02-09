namespace PaymentCard.API.Configurations
{
    public static class ApplicationInsightsConfig
    {
        public static void AddApplicationInsightsConfiguration(this IServiceCollection services, IHostEnvironment environment)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //  App Service → Configuration → Application settings = APPLICATIONINSIGHTS_CONNECTION_STRING
            if (!environment.IsDevelopment())
            {
                services.AddApplicationInsightsTelemetry();
            }
        }
    }
}