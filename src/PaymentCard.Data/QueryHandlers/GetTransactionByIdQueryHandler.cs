using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Contracts;
using PaymentCard.Data.CommandHandlers;
using PaymentCard.Data.Repositories;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static PaymentCard.Data.Queries.Queries;

namespace PaymentCard.Data.QueryHandlers
{
    public class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetTransactionByIdQueryHandler(

            ILogger<GetTransactionByIdQueryHandler> logger,
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetTransactionByIdQueryHandler), nameof(Handle));

            var data = await _transactionRepository.FindAsync(request.Id);

            var vm = _mapper.Map<TransactionResponse>(data);
            return vm;
        }
    }
}
