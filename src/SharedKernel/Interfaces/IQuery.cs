using MediatR;

namespace SharedKernel.Interfaces
{
    public interface IQuery<TResult> : IRequest<TResult>
    {
    }
}