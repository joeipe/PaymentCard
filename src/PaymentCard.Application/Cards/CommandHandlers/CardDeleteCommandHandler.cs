using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Data;
using static PaymentCard.Application.Cards.Commands.CardCommands;

namespace PaymentCard.Application.Cards.CommandHandlers
{
    public class CardDeleteCommandHandler : ICommandHandler<CardDeleteCommand>
    {
        private readonly ILogger<CardDeleteCommandHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CardDeleteCommandHandler(
            ILogger<CardDeleteCommandHandler> logger,
            ICardRepository cardRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CardDeleteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}({CardId}) start", nameof(CardDeleteCommand), nameof(Handle), request.Id);

            var data = await _cardRepository.FindAsync(request.Id);
            if (data != null)
            {
                _cardRepository.Delete(data);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}