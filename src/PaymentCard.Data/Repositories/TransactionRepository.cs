using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.Repositories;
using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        protected PaymentCardDbContext _dbContext;

        public TransactionRepository(
            ILogger<TransactionRepository> logger,
            PaymentCardDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<PurchaseTransaction> FindAsync(int id)
        {
            return await _dbContext.PurchaseTransactions.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<PurchaseTransaction>> GetAllAsync()
        {
            return await _dbContext.PurchaseTransactions.ToListAsync();
        }

        public void Create(params IEnumerable<PurchaseTransaction> items)
        {
            foreach (PurchaseTransaction item in items)
            {
                _dbContext.Add(item);
            }
        }

        public void Update(params IEnumerable<PurchaseTransaction> items)
        {
            foreach (PurchaseTransaction item in items)
            {
                _dbContext.Update(item);
            }
        }

        public void Delete(params IEnumerable<PurchaseTransaction> items)
        {
            foreach (PurchaseTransaction item in items)
            {
                _dbContext.Remove(item);
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}