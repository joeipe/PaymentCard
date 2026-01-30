using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Contracts.PurchaseTransactions;

namespace PaymentCard.Application.PurchaseTransactions.Commands
{
    public class TransactionCommands
    {
        public record TransactionCreateCommand(CreateTransactionRequest transaction) : ICommand { }
    }
}