using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Application.Interfaces.Services;
using PaymentCard.Contracts.Cards;
using static PaymentCard.Application.Cards.Queries.CardQueries;

namespace PaymentCard.Application.Cards.QueryHandlers
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

            if (data is not null)
            {
                var availableBalanceInUsd = data.CreditLimit - data.Transactions.Sum(t => t.Amount);
                vm.AvailableBalanceInUsd = Math.Round(availableBalanceInUsd, 2, MidpointRounding.AwayFromZero);

                if (!string.IsNullOrWhiteSpace(request.currency))
                {
                    var conversionResult = await _currencyConversionService.ConvertAmountToCurrencyAsync(request.currency, availableBalanceInUsd, DateTime.Now);
                    vm.ExchangeRateUsed = conversionResult.exchangeRateUsed;
                    vm.AvailableBalanceConverted = conversionResult.convertedAmount;
                    vm.TargetCurrency = conversionResult.targetCurrency;
                    vm.ErrorMessage = conversionResult.errorMessage;
                }
            }

            /*
            if (data is not null && !string.IsNullOrWhiteSpace(request.currency))
            {
                var conversionResult = await _currencyConversionService.ConvertTransactionsToCurrencyAsync(request.currency, data.Transactions);
                vm.TotalConvertedAmount = conversionResult.convertedAmount;
                vm.TargetCurrency = conversionResult.targetCurrency;
                vm.ErrorMessage = conversionResult.errorMessage;
            }*/

            return vm;
        }
    }
}