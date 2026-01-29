using MediatR;

namespace PaymentCard.Application.Interfaces.CQRS
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
    {
    }
}