using Microsoft.EntityFrameworkCore;
using PaymentCard.Domain;

namespace PaymentCard.Data
{
    public class PaymentCardDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }

        public PaymentCardDbContext()
        {
        }

        public PaymentCardDbContext(DbContextOptions<PaymentCardDbContext> options)
             : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Card).Assembly);
        }
    }
}