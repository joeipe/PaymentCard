using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Contracts.PurchaseTransactions;

namespace PaymentCard.Application.PurchaseTransactions.Queries
{
    public class TransactionQueries
    {
        public record GetTransactionsQuery() : IQuery<List<TransactionBaseResponse>>;
        public record GetTransactionByIdQuery(int Id, string? currency) : IQuery<TransactionResponse>;
    }
}