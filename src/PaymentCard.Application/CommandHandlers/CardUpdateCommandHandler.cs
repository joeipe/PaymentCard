using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Data;
using static PaymentCard.Application.Commands.Commands;

namespace PaymentCard.Application.CommandHandlers
{
    public class CardUpdateCommandHandler : ICommandHandler<CardUpdateCommand>
    {
        private readonly ILogger<CardUpdateCommandHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CardUpdateCommandHandler(
            ILogger<CardUpdateCommandHandler> logger,
            ICardRepository cardRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CardUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action}( start", nameof(CardUpdateCommandHandler), nameof(Handle));

            var data = await _cardRepository.FindAsync(request.Id);
            if (data != null)
            {
                _mapper.Map(request.card, data);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}