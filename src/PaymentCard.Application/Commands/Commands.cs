using PaymentCard.Contracts;
using SharedKernel.Interfaces;

namespace PaymentCard.Application.Commands
{
    public class Commands
    {
        public record CardCreateCommand(CreateCardRequest card) : ICommand { }
        public record CardUpdateCommand(int Id, UpdateCardRequest card) : ICommand { }
        public record CardDeleteCommand(int Id) : ICommand { }

        public record TransactionCreateCommand(CreateTransactionRequest transaction) : ICommand { }
    }
}