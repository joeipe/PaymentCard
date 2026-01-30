using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.Data.PurchaseTransactions
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
        }
    }
}