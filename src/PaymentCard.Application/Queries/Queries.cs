using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Contracts;

namespace PaymentCard.Application.Queries
{
    public class Queries
    {
        public record GetCardsQuery() : IQuery<List<CardResponse>>;
        public record GetCardByIdQuery(int Id) : IQuery<CardResponse>;
        public record GetCardBalanceQuery(int Id, string? currency) : IQuery<CardBalanceResponse>;

        public record GetTransactionsQuery() : IQuery<List<TransactionBaseResponse>>;
        public record GetTransactionByIdQuery(int Id, string? currency) : IQuery<TransactionResponse>;
    }
}