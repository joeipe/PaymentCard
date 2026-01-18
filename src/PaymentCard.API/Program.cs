using PaymentCard.API.Configurations;
using PaymentCard.Data.Queries;
using PaymentCard.Data.Repositories;
using PaymentCard.Data.Services;
using Serilog;
using System.Net.Http.Headers;

public partial class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Starting up");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddValidation();

            if (!IsRunningIntegrationTests())
            {
                builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration));
            }

            // Add services to the container.
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Queries).Assembly);
            });
            builder.Services.AddDatabaseConfiguration(builder.Configuration);
            builder.Services.AddAutoMapperConfiguration();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ICurrencyConversionService, CurrencyConversionService>();
            builder.Services.AddHttpClient<ICurrencyService, CurrencyService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiClient:TreasuryUri") ?? "http://localhost");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddExceptionConfiguration();

            builder.Services.AddAuthenticationConfiguration(builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerConfiguration();

            var app = builder.Build();

            app.ApplyException(app.Environment);

            app.UseHttpsRedirection();

            app.ApplySwagger();

            app.ApplyAuth();

            app.RegisterCardsEndpoints();
            app.RegisterTransactionsEndpoints();

            if (!IsRunningIntegrationTests())
            {
                app.ApplyDatabaseSchema(app.Environment);
            }

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unhandled exception");
        }
        finally
        {
            Log.Information("Shut down complete");
            Log.CloseAndFlush();
        }
    }

    private static bool IsRunningIntegrationTests()
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Any(a => a.FullName!.StartsWith("Microsoft.AspNetCore.Mvc.Testing"));
    }
}