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
        private readonly IMapper _mapper;

        public CardCreateCommandHandler(
            ILogger<CardCreateCommandHandler> logger,
            ICardRepository cardRepository,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task Handle(CardCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}( start", nameof(CardCreateCommandHandler), nameof(Handle));

            var data = _mapper.Map<Card>(request.card);
            _cardRepository.Create(data);
            await _cardRepository.SaveAsync();
        }
    }
}