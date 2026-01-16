using PaymentCard.Contracts;
using SharedKernel.Interfaces;

namespace PaymentCard.Data.Queries
{
    public class Queries
    {
        public record GetCardsQuery() : IQuery<List<CardResponse>>;
        public record GetCardByIdQuery(int Id) : IQuery<CardResponse>;

        public record GetTransactionsQuery() : IQuery<List<TransactionResponse>>;
        public record GetTransactionByIdQuery(int Id) : IQuery<TransactionResponse>;
    }
}