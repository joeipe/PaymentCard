using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Contracts;
using PaymentCard.Data.Repositories;
using SharedKernel.Interfaces;
using static PaymentCard.Data.Queries.Queries;

namespace PaymentCard.Data.QueryHandlers
{
    public class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, List<TransactionResponse>>
    {
        private readonly ILogger<GetTransactionsQueryHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(

            ILogger<GetTransactionsQueryHandler> logger,
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<List<TransactionResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetCardsQueryHandler), nameof(Handle));

            var data = await _transactionRepository.GetAllAsync();

            var vm = _mapper.Map<List<TransactionResponse>>(data);
            return vm;
        }
    }
}