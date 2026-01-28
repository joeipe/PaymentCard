using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentCard.Contracts;
using static PaymentCard.Application.Commands.Commands;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.API.EndpointsHandler
{
    public static class CardsHandler
    {
        public static async Task<Ok<List<CardResponse>>> GetCardsAsync(
            IMediator mediator)
        {
            var query = new GetCardsQuery();
            var result = await mediator.Send(query);

            return TypedResults.Ok(result);
        }

        public static async Task<Results<NotFound, Ok<CardResponse>>> GetCardsByIdAsync(
            IMediator mediator,
            int id)
        {
            var query = new GetCardByIdQuery(id);
            var result = await mediator.Send(query);

            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }

        public static async Task<Results<NotFound, Ok<CardBalanceResponse>>> GetCardBalanceAsync(
            IMediator mediator,
            int id,
            string? currency)
        {
            var query = new GetCardBalanceQuery(id, currency);
            var result = await mediator.Send(query);

            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }

        public static async Task<NoContent> CreateCardAsync(
           IMediator mediator,
           CreateCardRequest value)
        {
            var command = new CardCreateCommand(value);
            await mediator.Send(command);

            return TypedResults.NoContent();
        }

        public static async Task<NoContent> UpdateCardAsync(
           IMediator mediator,
           int id,
           UpdateCardRequest value)
        {
            var command = new CardUpdateCommand(id, value);
            await mediator.Send(command);

            return TypedResults.NoContent();
        }

        public static async Task<NoContent> DeleteCardAsync(
           IMediator mediator,
           int id)
        {
            var command = new CardDeleteCommand(id);
            await mediator.Send(command);

            return TypedResults.NoContent();
        }
    }
}