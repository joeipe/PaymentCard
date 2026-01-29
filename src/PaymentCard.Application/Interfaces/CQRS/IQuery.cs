using MediatR;

namespace PaymentCard.Application.Interfaces.CQRS
{
    public interface IQuery<TResult> : IRequest<TResult>
    {
    }
}