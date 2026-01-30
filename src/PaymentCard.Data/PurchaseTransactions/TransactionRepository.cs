using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Data.Shared;
using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.Data.PurchaseTransactions
{
    public class TransactionRepository : Repository<PurchaseTransaction>, ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        protected DatabaseContext _dbContext;

        public TransactionRepository(
            ILogger<TransactionRepository> logger,
            DatabaseContext dbContext)
            : base(dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}