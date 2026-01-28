using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.Interfaces;
using PaymentCard.Application.QueryHandlers;
using PaymentCard.Domain;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.UnitTests.QueryHandlers
{
    public class GetCardByIdQueryHandlerShould
    {
        private static Mock<ILogger<GetCardByIdQueryHandler>> _mockLogger = null!;
        private static Mock<ICardRepository> _mockCardRepository = null!;
        private static IMapper _mapper = null!;

        public GetCardByIdQueryHandlerShould()
        {
            _mockLogger = new Mock<ILogger<GetCardByIdQueryHandler>>();
            _mockCardRepository = new Mock<ICardRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CardProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task GetCardByIdSuccessfully()
        {
            // Arrange
            var data = new Card
            {
                Id = 1,
                CardNumber = "1234567890123456",
                CreditLimit = 100m
            };

            var id = 1;
            _mockCardRepository.Setup(x => x.FindAsync(id)).ReturnsAsync(data);

            // Act
            var sut = new GetCardByIdQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            var result = await sut.Handle(new GetCardByIdQuery(id), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.FindAsync(id), Times.Once);
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.CardNumber.Should().Be("1234567890123456");
            result.CreditLimit.Should().Be(100m);
        }

        [Fact]
        public async Task ReturnNullWhenCardNotFound()
        {
            // Arrange
            var id = 99;
            _mockCardRepository.Setup(x => x.FindAsync(id)).ReturnsAsync((Card?)null);

            // Act
            var sut = new GetCardByIdQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            var result = await sut.Handle(new GetCardByIdQuery(id), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.FindAsync(id), Times.Once);
            result.Should().BeNull();
        }
    }
}