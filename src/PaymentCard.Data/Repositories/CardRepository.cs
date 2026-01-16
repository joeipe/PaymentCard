using Microsoft.EntityFrameworkCore;
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

        public async Task<Card> GetCardByIdWithTransactionsAsync(int id)
        {
            _logger.LogInformation("{Repository}.{Action} start", nameof(CardRepository), nameof(GetCardByIdWithTransactionsAsync));

            var attendanceData = await SearchForIncludeAsync
                (
                    s => s.Id == id,
                    source => source
                        .Include(x => x.Transactions)
                );

            return attendanceData.First();
        }
    }
}