using Microsoft.OpenApi;

namespace PaymentCard.API.Configurations
{
    public static class SwaggerConfig
    {
        private const string schemeId = "bearer";

        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PaymentCard API",
                    Version = "v1"
                });

                options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description = "Token-based authentication and authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header
                });
                options.AddSecurityRequirement(document => new() { [new OpenApiSecuritySchemeReference(schemeId, document)] = [] });
            });
        }

        public static void ApplySwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}