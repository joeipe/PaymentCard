using PaymentCard.Domain;

namespace PaymentCard.Application.Interfaces.Data
{
    public interface ITransactionRepository : IRepository<PurchaseTransaction>
    {
    }
}