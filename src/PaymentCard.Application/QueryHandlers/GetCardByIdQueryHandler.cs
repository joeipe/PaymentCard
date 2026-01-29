using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.CQRS;
using PaymentCard.Application.Interfaces.Repositories;
using PaymentCard.Contracts;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.Application.QueryHandlers
{
    public class GetCardByIdQueryHandler : IQueryHandler<GetCardByIdQuery, CardResponse>
    {
        private readonly ILogger<GetCardByIdQueryHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public GetCardByIdQueryHandler(
            ILogger<GetCardByIdQueryHandler> logger,
            ICardRepository cardRepository,
            IMapper mapper)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task<CardResponse> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Handler}.{Action} start", nameof(GetCardByIdQueryHandler), nameof(Handle));

            var data = await _cardRepository.FindAsync(request.Id);

            var vm = _mapper.Map<CardResponse>(data);
            return vm;
        }
    }
}