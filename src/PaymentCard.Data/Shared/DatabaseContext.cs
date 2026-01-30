using Microsoft.EntityFrameworkCore;
using PaymentCard.Domain.Cards;
using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.Data.Shared
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
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