using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentCard.Application.Services;
using PaymentCard.Application.Services.models;
using PaymentCard.Data;
using PaymentCard.Domain;

namespace PaymentCard.IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureTestServices(services =>
            {
                // Register a test authentication scheme so endpoints that require authorization succeed in tests.
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

                var descriptor = services.SingleOrDefault
                   (d => d.ServiceType == typeof(DbContextOptions<PaymentCardDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add DataContext using an in-memory database for testing.
                var _connection = new SqliteConnection("Filename=:memory:");
                _connection.Open();
                services.AddDbContext<PaymentCardDbContext>(options =>
                    options.UseSqlite(_connection)
                );

                // ---- Replace ICurrencyService to use Mock ----
                var data = GetTreasuryExchangeRates();
                services.RemoveAll<ICurrencyService>();
                var mockCurrencyService = new Mock<ICurrencyService>(MockBehavior.Strict);
                mockCurrencyService.Setup(b => b.GetExchangeRatesAsync()).ReturnsAsync(data);
                services.AddScoped<ICurrencyService>(_ => mockCurrencyService.Object);

                // Build the service provider.
                var serviceProvider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<PaymentCardDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplicationFactory<Program>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    SeedInMemoryStore(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred seeding the database with test messages.Error: {ex.Message} ");
                }
            });
        }

        private void SeedInMemoryStore(PaymentCardDbContext context)
        {
            if (context.Cards.Any())
                return;

            // ---- Cards ----
            var card1 = new Card
            {
                Id = 1,
                CardNumber = "4111111111111111",
                CreditLimit = 1000
            };

            var card2 = new Card
            {
                Id = 2,
                CardNumber = "5500000000000004",
                CreditLimit = 2000
            };

            context.Cards.AddRange(card1, card2);

            // ---- Transactions ----
            var transactions = new List<PurchaseTransaction>
            {
                new PurchaseTransaction
                {
                    Id = 1,
                    CardId = 1,
                    Amount = 50,
                    Description = "Amazon Purchase"
                },
                new PurchaseTransaction
                {
                    Id = 2,
                    CardId = 1,
                    Amount = 120,
                    Description = "Fuel Station"
                },
                new PurchaseTransaction
                {
                    Id = 3,
                    CardId = 2,
                    Amount = 300,
                    Description = "JB Hi-Fi Electronics"
                },
                new PurchaseTransaction
                {
                    Id = 4,
                    CardId = 2,
                    Amount = 45,
                    Description = "Coffee Shop"
                }
            };

            context.PurchaseTransactions.AddRange(transactions);

            context.SaveChanges();
        }

        private List<TreasuryExchangeRateDto> GetTreasuryExchangeRates()
        {
            return new List<TreasuryExchangeRateDto>
            {
                new TreasuryExchangeRateDto
                {
                    RecordDate = DateTime.Now.AddDays(-2),
                    Country = "Algeria",
                    Currency = "Dinar",
                    CountryCurrencyDescription = "Algeria-Dinar",
                    ExchangeRate = 140.0m,
                    EffectiveDate = DateTime.Now.AddDays(-2),
                    SourceLineNumber = 1
                }
            };
        }
    }
}