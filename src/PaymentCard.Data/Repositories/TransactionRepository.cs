using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Data.Shared;
using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
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

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}