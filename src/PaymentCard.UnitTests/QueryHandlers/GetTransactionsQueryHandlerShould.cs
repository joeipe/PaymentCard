using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Application.QueryHandlers;
using PaymentCard.Domain;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.UnitTests.QueryHandlers
{
    public class GetTransactionsQueryHandlerShould
    {
        private static Mock<ILogger<GetTransactionsQueryHandler>> _mockLogger = null!;
        private static Mock<ITransactionRepository> _mockTransactionRepository = null!;
        private static IMapper _mapper = null!;

        public GetTransactionsQueryHandlerShould()
        {
            _mockLogger = new Mock<ILogger<GetTransactionsQueryHandler>>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TransactionProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task GetTransactionsSuccessfully()
        {
            // Arrange
            var transactions = new List<PurchaseTransaction>
            {
                new PurchaseTransaction
                {
                    Id = 1,
                    Description = "Test desc 1",
                    TransactionDate = new DateTime(2026, 01, 17),
                    Amount = 10m,
                    CardId = 1
                },
                new PurchaseTransaction
                {
                    Id = 2,
                    Description = "Test desc 2",
                    TransactionDate = new DateTime(2026, 01, 18),
                    Amount = 20m,
                    CardId = 1
                }
            };

            _mockTransactionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(transactions);

            // Act
            var sut = new GetTransactionsQueryHandler(_mockLogger.Object, _mockTransactionRepository.Object, _mapper);
            var result = await sut.Handle(new GetTransactionsQuery(), CancellationToken.None);

            // Assert
            _mockTransactionRepository.Verify(x => x.GetAllAsync(), Times.Once);
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].Description.Should().Be("Test desc 1");
            result[0].OriginalUsdAmount.Should().Be(10m);
            result[0].CardId.Should().Be(1);
        }
    }
}