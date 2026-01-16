using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Contracts;
using PaymentCard.Data.Repositories;
using PaymentCard.Data.Services;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using static PaymentCard.Data.Queries.Queries;
using static System.Net.Mime.MediaTypeNames;

namespace PaymentCard.Data.QueryHandlers
{
    public class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public GetTransactionByIdQueryHandler(

            ILogger<GetTransactionByIdQueryHandler> logger,
            ITransactionRepository transactionRepository,
            ICurrencyService currencyService,
            IMapper mapper)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _currencyService = currencyService;
            _mapper = mapper;
        }

        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetTransactionByIdQueryHandler), nameof(Handle));

            var data = await _transactionRepository.FindAsync(request.Id);

            var vm = _mapper.Map<TransactionResponse>(data);

            if (data is not null && !string.IsNullOrWhiteSpace(request.currency))
            {
                var exchangeRateResult = await _currencyService.GetExchangeRatesAsync();

                var cutoffDate = data.TransactionDate.AddMonths(-6);

                var selectedRate = exchangeRateResult?
                    .Where(r => r.CountryCurrencyDescription.ToLower() == request.currency.ToLower())
                    .Where(r => r.RecordDate <= data.TransactionDate)
                    .Where(r => r.RecordDate >= cutoffDate)
                    .OrderByDescending(r => r.RecordDate)
                    .FirstOrDefault();
                _logger.LogInformation(selectedRate?.OutputJson());

                vm.ExchangeRateUsed = selectedRate?.ExchangeRate;
                vm.ConvertedAmount = selectedRate is not null? Math.Round(data.Amount * selectedRate.ExchangeRate, 2, MidpointRounding.AwayFromZero) : null;
                vm.TargetCurrency = request.currency.ToUpperInvariant();
                vm.ErrorMessage = selectedRate is null ? $"No exchange rate found for currency {request.currency} within 6 months prior to transaction date." : null;
            }

            return vm;
        }
    }
}