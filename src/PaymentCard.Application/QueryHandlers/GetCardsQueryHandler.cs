using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces;
using PaymentCard.Contracts;
using SharedKernel.Interfaces;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.Application.QueryHandlers
{
    public class GetCardsQueryHandler : IQueryHandler<GetCardsQuery, List<CardResponse>>
    {
        private readonly ILogger<GetCardsQueryHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public GetCardsQueryHandler(
            ILogger<GetCardsQueryHandler> logger,
            ICardRepository cardRepository,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task<List<CardResponse>> Handle(GetCardsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetCardsQueryHandler), nameof(Handle));

            var data = await _cardRepository.GetAllAsync();

            var vm = _mapper.Map<List<CardResponse>>(data);
            return vm;
        }
    }
}