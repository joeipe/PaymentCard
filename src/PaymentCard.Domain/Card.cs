using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace PaymentCard.Domain
{
    public class Card : Entity, IEntityTypeConfiguration<Card>
    {
        public string CardNumber { get; set; } = default!;
        public decimal CreditLimit { get; set; }

        public List<PurchaseTransaction> Transactions { get; set; }

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
        }
    }
}