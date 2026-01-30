using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Domain.PurchaseTransactions;
using static PaymentCard.Application.PurchaseTransactions.Commands.TransactionCommands;

namespace PaymentCard.Application.PurchaseTransactions.CommandHandlers
{
    public class TransactionCreateCommandHandler : ICommandHandler<TransactionCreateCommand>
    {
        private readonly ILogger<TransactionCreateCommandHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionCreateCommandHandler(
            ILogger<TransactionCreateCommandHandler> logger,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(TransactionCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}( start", nameof(TransactionCreateCommand), nameof(Handle));

            var data = _mapper.Map<PurchaseTransaction>(request.transaction);
            _transactionRepository.Create(data);
            await _unitOfWork.SaveAsync();
        }
    }
}