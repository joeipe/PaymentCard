using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Data.Repositories;
using PaymentCard.Domain;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static PaymentCard.Data.Commands.Commands;

namespace PaymentCard.Data.CommandHandlers
{
    public class TransactionCreateCommandHandler : ICommandHandler<TransactionCreateCommand>
    {
        private readonly ILogger<TransactionCreateCommandHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionCreateCommandHandler(
            ILogger<TransactionCreateCommandHandler> logger,
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task Handle(TransactionCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}( start", nameof(TransactionCreateCommand), nameof(Handle));

            var data = _mapper.Map<PurchaseTransaction>(request.transaction);
            _transactionRepository.Create(data);
            await _transactionRepository.SaveAsync();
        }
    }
}
