using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.Interfaces.Repositories;
using PaymentCard.Application.Interfaces.Services;
using PaymentCard.Application.QueryHandlers;
using PaymentCard.Domain;
using static PaymentCard.Application.Queries.Queries;

namespace PaymentCard.UnitTests.QueryHandlers
{
    public class GetTransactionByIdQueryHandlerShould
    {
        private static Mock<ILogger<GetTransactionByIdQueryHandler>> _mockLogger = null!;
        private static Mock<ITransactionRepository> _mockTransactionRepository = null!;
        private static Mock<ICurrencyConversionService> _mockCurrencyConversionService = null!;
        private static IMapper _mapper = null!;

        public GetTransactionByIdQueryHandlerShould()
        {
            _mockLogger = new Mock<ILogger<GetTransactionByIdQueryHandler>>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockCurrencyConversionService = new Mock<ICurrencyConversionService>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TransactionProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task GetTransactionByIdSuccessfully()
        {
            //Arrange
            var data = new PurchaseTransaction
            {
                Id = 1,
                Description = "Tes desc",
                TransactionDate = new DateTime(2026, 01, 17),
                Amount = 10,
                CardId = 1
            };

            var id = 1;

            var currency = "Algeria-Dinar";
            _mockTransactionRepository.Setup(x => x.FindAsync(id)).ReturnsAsync(data);

            //Act
            var sut = new GetTransactionByIdQueryHandler(_mockLogger.Object, _mockTransactionRepository.Object, _mockCurrencyConversionService.Object, _mapper);
            var result = await sut.Handle(new GetTransactionByIdQuery(id, currency), new CancellationToken());

            //Assert
            _mockTransactionRepository.Verify(x => x.FindAsync(id), Times.Once);
            result.Id.Should().Be(1);
            result.Description.Should().Be("Tes desc");
        }
    }
}