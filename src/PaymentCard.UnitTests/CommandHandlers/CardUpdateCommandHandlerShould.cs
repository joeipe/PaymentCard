using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.CommandHandlers;
using PaymentCard.Application.Interfaces;
using PaymentCard.Contracts;
using PaymentCard.Domain;
using static PaymentCard.Application.Commands.Commands;

namespace PaymentCard.UnitTests.CommandHandlers
{
    public class CardUpdateCommandHandlerShould
    {
        private static Mock<ILogger<CardUpdateCommandHandler>> _mockLogger = null!;
        private static Mock<ICardRepository> _mockCardRepository = null!;
        private static IMapper _mapper = null!;

        public CardUpdateCommandHandlerShould()
        {
            _mockLogger = new Mock<ILogger<CardUpdateCommandHandler>>();
            _mockCardRepository = new Mock<ICardRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CardProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task UpdateExistingCard_ShouldMapFieldsAndSave()
        {
            // Arrange
            var existingCard = new Card
            {
                Id = 1,
                CardNumber = "4111111111111111",
                CreditLimit = 100m
            };

            var updateRequest = new UpdateCardRequest
            {
                CreditLimit = 200m
            };

            _mockCardRepository.Setup(x => x.FindAsync(existingCard.Id)).ReturnsAsync(existingCard);

            // Act
            var sut = new CardUpdateCommandHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            await sut.Handle(new CardUpdateCommand(existingCard.Id, updateRequest), CancellationToken.None);

            // Assert - mapping changed the instance returned by FindAsync
            Assert.Equal(updateRequest.CreditLimit, existingCard.CreditLimit);

            _mockCardRepository.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateNonExistingCard_ShouldNotCallSave()
        {
            // Arrange
            _mockCardRepository.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync((Card?)null);

            var updateRequest = new UpdateCardRequest
            {
                CreditLimit = 200m
            };

            // Act
            var sut = new CardUpdateCommandHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            await sut.Handle(new CardUpdateCommand(1, updateRequest), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}