using PaymentCard.Domain.Cards;

namespace PaymentCard.Application.Interfaces.Data

{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card?> GetCardByIdWithTransactionsAsync(int id);
    }
}