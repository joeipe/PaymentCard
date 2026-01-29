using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Data.Shared;
using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        private readonly ILogger<CardRepository> _logger;
        private readonly DatabaseContext _dbContext;

        public CardRepository(
            ILogger<CardRepository> logger,
            DatabaseContext dbContext)
            : base(dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Card?> GetCardByIdWithTransactionsAsync(int id)
        {
            _logger.LogInformation("{Repository}.{Action} start", nameof(CardRepository), nameof(GetCardByIdWithTransactionsAsync));

            var cardData = await _dbContext.Cards
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == id);

            return cardData;
        }
    }
}