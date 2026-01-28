using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentCard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Data.Configuration
{
    public class PurchaseTransactionConfiguration : IEntityTypeConfiguration<PurchaseTransaction>
    {
        public void Configure(EntityTypeBuilder<PurchaseTransaction> builder)
        {
            builder.Property(e => e.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.TransactionDate)
                .IsRequired();

            builder.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasData(
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
        }
    }
}
