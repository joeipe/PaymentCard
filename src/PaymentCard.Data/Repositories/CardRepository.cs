using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.Repositories;
using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ILogger<CardRepository> _logger;
        protected PaymentCardDbContext _dbContext;

        public CardRepository(
            ILogger<CardRepository> logger,
            PaymentCardDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Card?> FindAsync(int id)
        {
            return await _dbContext.Cards.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Card>> GetAllAsync()
        {
            return await _dbContext.Cards.ToListAsync();
        }

        public async Task<Card?> GetCardByIdWithTransactionsAsync(int id)
        {
            _logger.LogInformation("{Repository}.{Action} start", nameof(CardRepository), nameof(GetCardByIdWithTransactionsAsync));

            var cardData = await _dbContext.Cards
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == id);

            return cardData;
        }

        public void Create(params IEnumerable<Card> items)
        {
            foreach (Card item in items)
            {
                _dbContext.Add(item);
            }
        }

        public void Update(params IEnumerable<Card> items)
        {
            foreach (Card item in items)
            {
                _dbContext.Update(item);
            }
        }

        public void Delete(params IEnumerable<Card> items)
        {
            foreach (Card item in items)
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