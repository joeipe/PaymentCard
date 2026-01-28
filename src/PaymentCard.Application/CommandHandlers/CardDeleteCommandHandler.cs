using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces;
using SharedKernel.Interfaces;
using static PaymentCard.Application.Commands.Commands;

namespace PaymentCard.Application.CommandHandlers
{
    public class CardDeleteCommandHandler : ICommandHandler<CardDeleteCommand>
    {
        private readonly ILogger<CardDeleteCommandHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardDeleteCommandHandler(
            ILogger<CardDeleteCommandHandler> logger,
            ICardRepository cardRepository,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task Handle(CardDeleteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}({CardId}) start", nameof(CardDeleteCommand), nameof(Handle), request.Id);

            var data = await _cardRepository.FindAsync(request.Id);
            if (data != null)
            {
                _cardRepository.Delete(data);
                await _cardRepository.SaveAsync();
            }
        }
    }
}