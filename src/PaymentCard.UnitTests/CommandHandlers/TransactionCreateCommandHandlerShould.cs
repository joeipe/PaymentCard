using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.CommandHandlers;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Contracts;
using PaymentCard.Domain;
using static PaymentCard.Application.Commands.Commands;

namespace PaymentCard.UnitTests.CommandHandlers
{
    public class TransactionCreateCommandHandlerShould
    {
        private static Mock<ILogger<TransactionCreateCommandHandler>> _mockLogger = null!;
        private static Mock<ITransactionRepository> _mockTransactionRepository = null!;
        private static Mock<IUnitOfWork> _mockUnitOfWork = null!;
        private static IMapper _mapper = null!;

        public TransactionCreateCommandHandlerShould()
        {
            _mockLogger = new Mock<ILogger<TransactionCreateCommandHandler>>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TransactionProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task CreateNewTransaction_ShouldCallCreateAndSave()
        {
            // Arrange
            var data = new CreateTransactionRequest
            {
                Description = "Test purchase",
                TransactionDate = new DateTime(2026, 1, 17),
                Amount = 10m,
                CardId = 1
            };

            // Act
            var sut = new TransactionCreateCommandHandler(_mockLogger.Object, _mockTransactionRepository.Object, _mockUnitOfWork.Object, _mapper);
            await sut.Handle(new TransactionCreateCommand(data), CancellationToken.None);

            // Assert
            _mockTransactionRepository.Verify(x => x.Create(It.IsAny<IEnumerable<PurchaseTransaction>>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}