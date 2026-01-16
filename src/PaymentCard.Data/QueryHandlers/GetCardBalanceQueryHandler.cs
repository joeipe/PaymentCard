using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Contracts;
using PaymentCard.Data.Repositories;
using PaymentCard.Data.Services;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static PaymentCard.Data.Queries.Queries;

namespace PaymentCard.Data.QueryHandlers
{
    public class GetCardBalanceQueryHandler : IQueryHandler<GetCardBalanceQuery, CardBalanceResponse>
    {
        private readonly ILogger<GetCardBalanceQueryHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public GetCardBalanceQueryHandler(
            ILogger<GetCardBalanceQueryHandler> logger,
            ICardRepository cardRepository,
            ICurrencyService currencyService,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _currencyService = currencyService;
            _mapper = mapper;
        }

        public async Task<CardBalanceResponse> Handle(GetCardBalanceQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetCardBalanceQueryHandler), nameof(Handle));

            var data = await _cardRepository.GetCardByIdWithTransactionsAsync(request.Id);

            var vm = _mapper.Map<CardBalanceResponse>(data);

            var availableBalanceInUsd = data.CreditLimit - data.Transactions.Sum(t => t.Amount);
            vm.AvailableBalanceInUsd = Math.Round(availableBalanceInUsd, 2, MidpointRounding.AwayFromZero);

            if (data is not null && !string.IsNullOrWhiteSpace(request.currency))
            {
                var exchangeRateResult = await _currencyService.GetExchangeRatesAsync();

                var convertedAmount = 0.0m;
                foreach (var transaction in data.Transactions)
                {
                    var cutoffDate = transaction.TransactionDate.AddMonths(-6);

                    var selectedRate = exchangeRateResult?
                        .Where(r => r.CountryCurrencyDescription.ToLower() == request.currency.ToLower())
                        .Where(r => r.RecordDate <= transaction.TransactionDate)
                        .Where(r => r.RecordDate >= cutoffDate)
                        .OrderByDescending(r => r.RecordDate)
                        .FirstOrDefault();

                    convertedAmount += selectedRate is not null ? Math.Round(transaction.Amount * selectedRate.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0m;

                    if (selectedRate is null)
                    {
                        convertedAmount = 0.0m;
                        break;
                    }
                }
                vm.AvailableBalanceConverted = Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero);
                vm.TargetCurrency = request.currency.ToUpperInvariant();
                vm.ErrorMessage = convertedAmount == 0.0m ? $"No exchange rates found for currency {request.currency} within 6 months prior to transaction dates." : null;
            }

            return vm;
        }
    }
}
