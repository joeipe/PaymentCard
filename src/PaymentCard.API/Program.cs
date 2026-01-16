using PaymentCard.API.Configurations;
using PaymentCard.Data.Queries;
using PaymentCard.Data.Repositories;
using Serilog;

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

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration));

            // Add services to the container.
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Queries).Assembly);
            });
            builder.Services.AddDatabaseConfiguration(builder.Configuration);
            builder.Services.AddAutoMapperConfiguration();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddExceptionConfiguration();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerConfiguration();

            var app = builder.Build();

            app.ApplyException(app.Environment);

            app.UseHttpsRedirection();

            app.ApplySwagger();

            app.RegisterCardsEndpoints();
            app.RegisterTransactionsEndpoints();

            app.ApplyDatabaseSchema(app.Environment);

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
}