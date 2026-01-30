using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Contracts.Cards;

namespace PaymentCard.Application.Cards.Commands
{
    public class CardCommands
    {
        public record CardCreateCommand(CreateCardRequest card) : ICommand { }
        public record CardUpdateCommand(int Id, UpdateCardRequest card) : ICommand { }
        public record CardDeleteCommand(int Id) : ICommand { }
    }
}