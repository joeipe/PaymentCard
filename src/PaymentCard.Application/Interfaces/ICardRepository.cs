using PaymentCard.Domain;

namespace PaymentCard.Application.Interfaces

{
    public interface ICardRepository
    {
        Task<Card> FindAsync(int id);

        Task<IEnumerable<Card>> GetAllAsync();

        Task<Card> GetCardByIdWithTransactionsAsync(int id);

        void Create(params IEnumerable<Card> items);

        void Update(params IEnumerable<Card> items);

        void Delete(params IEnumerable<Card> items);

        Task SaveAsync();
    }
}