using PaymentCard.Domain;

namespace PaymentCard.Application.Interfaces.Data

{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card?> GetCardByIdWithTransactionsAsync(int id);
    }
}