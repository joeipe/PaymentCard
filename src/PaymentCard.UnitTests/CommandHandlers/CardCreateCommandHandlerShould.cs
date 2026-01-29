using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.CommandHandlers;
using PaymentCard.Application.Interfaces.Repositories;
using PaymentCard.Contracts;
using PaymentCard.Domain;
using static PaymentCard.Application.Commands.Commands;

namespace PaymentCard.UnitTests.CommandHandlers
{
    public class CardCreateCommandHandlerShould
    {
        private static Mock<ILogger<CardCreateCommandHandler>> _mockLogger = null!;
        private static Mock<ICardRepository> _mockCardRepository = null!;
        private static IMapper _mapper = null!;

        public CardCreateCommandHandlerShould()
        {
            _mockLogger = new Mock<ILogger<CardCreateCommandHandler>>();
            _mockCardRepository = new Mock<ICardRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CardProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task CreateNewCard()
        {
            //Arrange
            var data = new CreateCardRequest
            {
                CardNumber = "4111111111111111",
                CreditLimit = 100,
            };

            //Act
            var sut = new CardCreateCommandHandler(_mockLogger.Object, _mockCardRepository.Object, _mapper);
            await sut.Handle(new CardCreateCommand(data), new CancellationToken());

            //Assert
            _mockCardRepository.Verify(x => x.Create(It.IsAny<IEnumerable<Card>>()), Times.Once);
            _mockCardRepository.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}