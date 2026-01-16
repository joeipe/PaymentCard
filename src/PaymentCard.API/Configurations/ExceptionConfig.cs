namespace PaymentCard.API.Configurations
{
    public static class ExceptionConfig
    {
        public static void AddExceptionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddProblemDetails();
        }

        public static void ApplyException(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }
        }
    }
}