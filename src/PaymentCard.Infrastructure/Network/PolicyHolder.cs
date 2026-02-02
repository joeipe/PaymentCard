using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;
using System.Net;

namespace PaymentCard.Infrastructure.Network
{
    public class PolicyHolder
    {
        private readonly AsyncFallbackPolicy<HttpResponseMessage> _fallbackPolicy;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _breakerPolicy;

        public AsyncPolicyWrap<HttpResponseMessage> PolicyWrap { get; private set; }

        public PolicyHolder(ILogger<PolicyHolder> logger)
        {
            _fallbackPolicy = Policy
                .HandleResult<HttpResponseMessage>((r => !r.IsSuccessStatusCode))
                .Or<HttpRequestException>()
                .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK),
                    onFallbackAsync: async (outcome, context) =>
                    {
                        if (outcome.Exception is HttpRequestException ex)
                        {
                            logger.LogWarning("Fallback due to HTTP failure: {Error}", ex.Message);
                        }
                        else if (outcome.Result is HttpResponseMessage response)
                        {
                            logger.LogWarning("Fallback due to {StatusCode}", outcome.Result.StatusCode);
                        }

                        await Task.CompletedTask;
                    });

            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(
                        3,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt / 2)),
                        async (outcome, timespan, retryCount, context) =>
                        {
                            //response.Result.RequestMessage
                            logger.LogInformation($"Retry #{retryCount} \nReason: {outcome.Result?.StatusCode} {outcome.Result?.ReasonPhrase} Will wait: {timespan}");

                            //Context Logging
                            if (context.ContainsKey("Action"))
                            {
                                logger.LogInformation($"Context: {context["Action"]}");
                            }

                            // Error logging
                            if (outcome.Exception is HttpRequestException ex)
                            {
                                logger.LogWarning($"[Retry {retryCount}] Network error: {ex.Message}");
                            }
                            else if (outcome.Result is HttpResponseMessage response)
                            {
                                logger.LogWarning($"[Retry {retryCount}] HTTP failure: {response.StatusCode}");

                                if (!outcome.Result.IsSuccessStatusCode)
                                {
                                    if (outcome.Result.StatusCode == HttpStatusCode.UnprocessableEntity)
                                    {
                                        var errors = await outcome.Result.Content.ReadAsStringAsync();
                                        logger.LogInformation($"Error: {errors}");
                                    }
                                    else if (outcome.Result.StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        logger.LogInformation("Unauthorized");
                                    }
                                    //response.Result.EnsureSuccessStatusCode();
                                }
                            }
                        }
                    );

            _breakerPolicy = Policy<HttpResponseMessage>
                //.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Handle<HttpRequestException>()
                .AdvancedCircuitBreakerAsync(
                    0.5,
                    TimeSpan.FromSeconds(60),
                    7,
                    TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        if (outcome.Exception is HttpRequestException ex)
                        {
                            logger.LogWarning("Circuit breaker opened due to HTTP request failure: {Message}", ex.Message);
                        }
                        else
                        {
                            logger.LogWarning("Circuit breaker opened due to unknown error.");
                        }
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit breaker reset");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogInformation("Circuit is half-open. Testing the waters...");
                    });

            //PolicyWrap = Policy.WrapAsync(_fallbackPolicy, _retryPolicy, _breakerPolicy);
            PolicyWrap = Policy.WrapAsync(_retryPolicy, _breakerPolicy);
        }
    }
}