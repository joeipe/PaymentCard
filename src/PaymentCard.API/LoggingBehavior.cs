using MediatR;

namespace PaymentCard.API
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestName"] = typeof(TRequest).Name,
                ["RequestType"] = typeof(TRequest).FullName!,
                ["CorrelationId"] = Guid.NewGuid() // or from HttpContext
            }))
            {
                _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

                var response = await next();

                _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);

                return response;
            }
        }
    }
}