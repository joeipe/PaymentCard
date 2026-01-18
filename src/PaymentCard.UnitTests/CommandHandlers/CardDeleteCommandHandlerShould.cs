using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Data.CommandHandlers;
using PaymentCard.Data.Repositories;
using PaymentCard.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using static PaymentCard.Data.Commands.Commands;

namespace PaymentCard.UnitTests.CommandHandlers
{
    public class CardDeleteCommandHandlerShould
    {
        private static Mock<ILogger<CardDeleteCommandHandler>> _mockLogger = null!;
        private static Mock<ICardRepository> _mockCardRepository = null!;
        private static IMapper _mapper = null!;

        public CardDeleteCommandHandlerShould()
        {
            _mockLogger = new Mock<ILogger<CardDeleteCommandHandler>>();
            _mockCardRepository = new Mock<ICardRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CardProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task DeleteExistingCard_ShouldCallDeleteAndSave()
        {
            // Arrange
            var card = new Card
            {
                Id = 1,
                CardNumber = "4111111111111111",
                CreditLimit = 100m
            };

            _mockCardRepository.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(card);

            // Act
            var sut = new CardDeleteCommandHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            await sut.Handle(new CardDeleteCommand(card.Id), new CancellationToken());

            // Assert
            _mockCardRepository.Verify(x => x.Delete(It.IsAny<IEnumerable<Card>>()), Times.Once);
            _mockCardRepository.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteNonExistingCard_ShouldNotCallDeleteOrSave()
        {
            // Arrange
            _mockCardRepository.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync((Card?)null);

            // Act
            var sut = new CardDeleteCommandHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            await sut.Handle(new CardDeleteCommand(1), new CancellationToken());

            // Assert
            _mockCardRepository.Verify(x => x.Delete(It.IsAny<IEnumerable<Card>>()), Times.Never);
            _mockCardRepository.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}
