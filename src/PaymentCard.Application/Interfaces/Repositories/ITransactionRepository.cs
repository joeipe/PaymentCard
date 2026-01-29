using PaymentCard.Domain;

namespace PaymentCard.Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<PurchaseTransaction> FindAsync(int id);

        Task<IEnumerable<PurchaseTransaction>> GetAllAsync();

        void Create(params IEnumerable<PurchaseTransaction> items);

        void Update(params IEnumerable<PurchaseTransaction> items);

        void Delete(params IEnumerable<PurchaseTransaction> items);

        Task SaveAsync();
    }
}