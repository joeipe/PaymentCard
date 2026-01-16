using Microsoft.Extensions.Logging;
using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public class TransactionRepository : GenericRepository<PurchaseTransaction>, ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        protected PaymentCardDbContext _dbContext;

        public TransactionRepository(
            ILogger<TransactionRepository> logger,
            PaymentCardDbContext dbContext)
            : base(dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}