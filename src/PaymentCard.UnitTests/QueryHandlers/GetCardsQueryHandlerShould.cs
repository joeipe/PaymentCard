using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.QueryHandlers;
using PaymentCard.Data.Repositories;
using PaymentCard.Domain;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.UnitTests.QueryHandlers
{
    public class GetCardsQueryHandlerShould
    {
        private static Mock<ILogger<GetCardsQueryHandler>> _mockLogger = null!;
        private static Mock<ICardRepository> _mockCardRepository = null!;
        private static IMapper _mapper = null!;

        public GetCardsQueryHandlerShould()
        {
            _mockLogger = new Mock<ILogger<GetCardsQueryHandler>>();
            _mockCardRepository = new Mock<ICardRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CardProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task GetCardsSuccessfully_ReturnsMappedList()
        {
            // Arrange
            var cards = new List<Card>
            {
                new Card { Id = 1, CardNumber = "1111222233334444", CreditLimit = 100m },
                new Card { Id = 2, CardNumber = "5555666677778888", CreditLimit = 200m }
            };

            _mockCardRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cards);

            // Act
            var sut = new GetCardsQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            var result = await sut.Handle(new GetCardsQuery(), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.GetAllAsync(), Times.Once);
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].CardNumber.Should().Be("1111222233334444");
            result[0].CreditLimit.Should().Be(100m);
            result[1].Id.Should().Be(2);
        }

        [Fact]
        public async Task GetCards_WhenNoCards_ReturnsEmptyList()
        {
            // Arrange
            var empty = new List<Card>();
            _mockCardRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(empty);

            // Act
            var sut = new GetCardsQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            var result = await sut.Handle(new GetCardsQuery(), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.GetAllAsync(), Times.Once);
            result.Should().BeEmpty();
        }
    }
}