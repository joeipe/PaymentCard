using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Domain;
using static PaymentCard.Application.Commands.Commands;

namespace PaymentCard.Application.CommandHandlers
{
    public class CardCreateCommandHandler : ICommandHandler<CardCreateCommand>
    {
        private readonly ILogger<CardCreateCommandHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CardCreateCommandHandler(
            ILogger<CardCreateCommandHandler> logger,
            ICardRepository cardRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CardCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}( start", nameof(CardCreateCommandHandler), nameof(Handle));

            var data = _mapper.Map<Card>(request.card);
            _cardRepository.Create(data);
            await _unitOfWork.SaveAsync();
        }
    }
}