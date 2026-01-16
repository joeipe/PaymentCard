using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentCard.Contracts;
using static PaymentCard.Data.Commands.Commands;
using static PaymentCard.Data.Queries.Queries;

namespace PaymentCard.API.EndpointsHandler
{
    public static class TransactionsHandler
    {
        public static async Task<Ok<List<TransactionBaseResponse>>> GetTransactiosAsync(
           IMediator mediator)
        {
            var query = new GetTransactionsQuery();
            var result = await mediator.Send(query);

            return TypedResults.Ok(result);
        }

        public static async Task<Results<NotFound, Ok<TransactionResponse>>> GetTransactiosByIdAsync(
            IMediator mediator,
            int id,
            string? currency)
        {
            var query = new GetTransactionByIdQuery(id, currency);
            var result = await mediator.Send(query);

            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }

        public static async Task<NoContent> CreateTransactionAsync(
           IMediator mediator,
           CreateTransactionRequest value)
        {
            var command = new TransactionCreateCommand(value);
            await mediator.Send(command);

            return TypedResults.NoContent();
        }
    }
}