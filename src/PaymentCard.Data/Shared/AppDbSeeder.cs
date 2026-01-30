using Microsoft.EntityFrameworkCore;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Domain.Cards;
using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.Data.Shared
{
    public class AppDbSeeder : IDbSeeder
    {
        protected DatabaseContext _dbContext;

        public AppDbSeeder(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            if (!_dbContext.Cards.Any())
            {
                _dbContext.Cards.AddRange(
                    new Card
                    {
                        Id = 1,
                        CardNumber = "1234567812345678",
                        CreditLimit = 5000.00m
                    }
                );

                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.PurchaseTransactions.Any())
            {
                _dbContext.PurchaseTransactions.AddRange(
                    new PurchaseTransaction
                    {
                        Id = 1,
                        Description = "Grocery Shopping",
                        TransactionDate = new DateTime(2024, 1, 15),
                        Amount = 150.75m,
                        CardId = 1
                    },
                    new PurchaseTransaction
                    {
                        Id = 2,
                        Description = "Online Subscription",
                        TransactionDate = new DateTime(2024, 2, 10),
                        Amount = 9.99m,
                        CardId = 1
                    },
                    new PurchaseTransaction
                    {
                        Id = 3,
                        Description = "Restaurant Bill",
                        TransactionDate = new DateTime(2024, 3, 5),
                        Amount = 45.50m,
                        CardId = 1
                    }
                );

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}