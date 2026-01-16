using PaymentCard.API.EndpointsHandler;

namespace PaymentCard.API.Configurations
{
    public static class EndpointRouteBuilderConfig
    {
        public static void RegisterCardsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var cardsEndpoints = endpoints.MapGroup("/cards");

            cardsEndpoints.MapGet("", CardsHandler.GetCardsAsync)
                .WithSummary("Get all cards");

            cardsEndpoints.MapGet("/{id:int}", CardsHandler.GetCardsByIdAsync)
                .WithSummary("Get a card by providing an id");

            cardsEndpoints.MapPost("", CardsHandler.CreateCardAsync)
                .ProducesValidationProblem(400)
                .WithSummary("Create card");

            cardsEndpoints.MapPut("/{id:int}", CardsHandler.UpdateCardAsync)
                .ProducesValidationProblem(400)
                .WithSummary("Update card");

            cardsEndpoints.MapDelete("/{id:int}", CardsHandler.DeleteCardAsync)
                .WithSummary("Delete a card by providing Id");
        }

        public static void RegisterTransactionsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var transactionsEndpoints = endpoints.MapGroup("/transactions");

            transactionsEndpoints.MapGet("", TransactionsHandler.GetTransactiosAsync)
                .WithSummary("Get all transactions");

            transactionsEndpoints.MapGet("/{id:int}", TransactionsHandler.GetTransactiosByIdAsync)
                .WithSummary("Get a transaction by providing an id");

            transactionsEndpoints.MapPost("", TransactionsHandler.CreateTransactionAsync)
                .ProducesValidationProblem(400)
                .WithSummary("Create transaction");
        }
    }
}