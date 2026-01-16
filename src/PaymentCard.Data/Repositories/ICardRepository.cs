using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public interface ICardRepository : IGenericRepository<Card>
    {
        Task<Card> GetCardByIdWithTransactionsAsync(int id);
    }
}