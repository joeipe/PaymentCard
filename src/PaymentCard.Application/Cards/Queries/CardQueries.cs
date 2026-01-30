using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Contracts.Cards;

namespace PaymentCard.Application.Cards.Queries
{
    public class CardQueries
    {
        public record GetCardsQuery() : IQuery<List<CardResponse>>;
        public record GetCardByIdQuery(int Id) : IQuery<CardResponse>;
        public record GetCardBalanceQuery(int Id, string? currency) : IQuery<CardBalanceResponse>;
    }
}