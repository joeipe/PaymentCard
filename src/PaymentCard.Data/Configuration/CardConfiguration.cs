using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentCard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Data.Configuration
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(e => e.CreditLimit)
                .HasPrecision(18, 2)
                .IsRequired();

            builder
               .HasMany(c => c.Transactions)
               .WithOne(e => e.Card)
               .HasForeignKey(e => e.CardId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Card
                {
                    Id = 1,
                    CardNumber = "1234567812345678",
                    CreditLimit = 5000.00m
                }
            );
        }
    }
}
