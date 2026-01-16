using Microsoft.Extensions.Logging;
using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        private readonly ILogger<CardRepository> _logger;
        protected PaymentCardDbContext _dbContext;

        public CardRepository(
            ILogger<CardRepository> logger,
            PaymentCardDbContext dbContext)
            : base(dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}