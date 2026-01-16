using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Data.Repositories;
using SharedKernel.Interfaces;
using static PaymentCard.Data.Commands.Commands;

namespace PaymentCard.Data.CommandHandlers
{
    public class CardUpdateCommandHandler : ICommandHandler<CardUpdateCommand>
    {
        private readonly ILogger<CardUpdateCommandHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardUpdateCommandHandler(
            ILogger<CardUpdateCommandHandler> logger,
            ICardRepository cardRepository,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task Handle(CardUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}( start", nameof(CardUpdateCommandHandler), nameof(Handle));

            var data = await _cardRepository.FindAsync(request.Id);
            _mapper.Map(request.card, data);
            await _cardRepository.SaveAsync();
        }
    }
}