using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Contracts;
using PaymentCard.Data.Repositories;
using SharedKernel.Interfaces;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.Application.QueryHandlers
{
    public class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, List<TransactionBaseResponse>>
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

        public async Task<List<TransactionBaseResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetCardsQueryHandler), nameof(Handle));

            var data = await _transactionRepository.GetAllAsync();

            var vm = _mapper.Map<List<TransactionBaseResponse>>(data);
            return vm;
        }
    }
}