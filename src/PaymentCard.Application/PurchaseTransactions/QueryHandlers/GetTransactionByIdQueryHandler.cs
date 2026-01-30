using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Application.Interfaces.Services;
using PaymentCard.Contracts.PurchaseTransactions;
using static PaymentCard.Application.PurchaseTransactions.Queries.TransactionQueries;

namespace PaymentCard.Application.PurchaseTransactions.QueryHandlers
{
    public class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly IMapper _mapper;

        public GetTransactionByIdQueryHandler(

            ILogger<GetTransactionByIdQueryHandler> logger,
            ITransactionRepository transactionRepository,
            ICurrencyConversionService currencyConversionService,
            IMapper mapper)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _currencyConversionService = currencyConversionService;
            _mapper = mapper;
        }

        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetTransactionByIdQueryHandler), nameof(Handle));

            var data = await _transactionRepository.FindAsync(request.Id);

            var vm = _mapper.Map<TransactionResponse>(data);

            if (data is not null && !string.IsNullOrWhiteSpace(request.currency))
            {
                var conversionResult = await _currencyConversionService.ConvertTransactionsToCurrencyAsync(request.currency, data);
                vm.ExchangeRateUsed = conversionResult.exchangeRateUsed;
                vm.ConvertedAmount = conversionResult.convertedAmount;
                vm.TargetCurrency = conversionResult.targetCurrency;
                vm.ErrorMessage = conversionResult.errorMessage;
            }

            return vm;
        }
    }
}