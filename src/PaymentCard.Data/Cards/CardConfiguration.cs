using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentCard.Domain.Cards;

namespace PaymentCard.Data.Cards
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
        }
    }
}