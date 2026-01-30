using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.Application.Interfaces.Data
{
    public interface ITransactionRepository : IRepository<PurchaseTransaction>
    {
    }
}