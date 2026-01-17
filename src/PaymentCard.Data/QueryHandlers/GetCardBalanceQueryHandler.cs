using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Contracts;
using PaymentCard.Data.Repositories;
using PaymentCard.Data.Services;
using SharedKernel.Interfaces;
using static PaymentCard.Data.Queries.Queries;

namespace PaymentCard.Data.QueryHandlers
{
    public class GetCardBalanceQueryHandler : IQueryHandler<GetCardBalanceQuery, CardBalanceResponse>
    {
        private readonly ILogger<GetCardBalanceQueryHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly IMapper _mapper;

        public GetCardBalanceQueryHandler(
            ILogger<GetCardBalanceQueryHandler> logger,
            ICardRepository cardRepository,
            ICurrencyConversionService currencyConversionService,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _currencyConversionService = currencyConversionService;
            _mapper = mapper;
        }

        public async Task<CardBalanceResponse> Handle(GetCardBalanceQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetCardBalanceQueryHandler), nameof(Handle));

            var data = await _cardRepository.GetCardByIdWithTransactionsAsync(request.Id);

            var vm = _mapper.Map<CardBalanceResponse>(data);

            var availableBalanceInUsd = data.CreditLimit - data.Transactions.Sum(t => t.Amount);
            vm.AvailableBalanceInUsd = Math.Round(availableBalanceInUsd, 2, MidpointRounding.AwayFromZero);
            // to calculate the converted available balance i need to know which transaction date to consider

            if (data is not null && !string.IsNullOrWhiteSpace(request.currency))
            {
                var conversionResult = await _currencyConversionService.ConvertTransactionsToCurrencyAsync(request.currency, data.Transactions);
                vm.ExchangeRateUsed = conversionResult.exchangeRateUsed;
                vm.TotalConvertedAmount = conversionResult.convertedAmount;
                vm.TargetCurrency = conversionResult.targetCurrency;
                vm.ErrorMessage = conversionResult.errorMessage;
            }

            return vm;
        }
    }
}