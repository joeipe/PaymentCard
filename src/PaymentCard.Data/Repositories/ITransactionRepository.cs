using PaymentCard.Domain;

namespace PaymentCard.Data.Repositories
{
    public interface ITransactionRepository : IGenericRepository<PurchaseTransaction>
    {
    }
}