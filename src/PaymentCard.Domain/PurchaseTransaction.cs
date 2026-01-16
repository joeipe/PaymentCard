using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Domain
{
    public class PurchaseTransaction : Entity, IEntityTypeConfiguration<PurchaseTransaction>
    {
        public string Description { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; } = default!;

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
        }
    }
}
