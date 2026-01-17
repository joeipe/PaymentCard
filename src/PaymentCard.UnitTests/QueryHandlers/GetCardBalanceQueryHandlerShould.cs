using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Data.QueryHandlers;
using PaymentCard.Data.Repositories;
using PaymentCard.Data.Services;
using PaymentCard.Domain;
using static PaymentCard.Data.Queries.Queries;

namespace PaymentCard.UnitTests.QueryHandlers
{
    public class GetCardBalanceQueryHandlerShould
    {
        private static Mock<ILogger<GetCardBalanceQueryHandler>> _mockLogger = null!;
        private static Mock<ICardRepository> _mockCardRepository = null!;
        private static Mock<ICurrencyConversionService> _mockCurrencyConversionService = null!;
        private static IMapper _mapper = null!;

        public GetCardBalanceQueryHandlerShould()
        {
            _mockLogger = new Mock<ILogger<GetCardBalanceQueryHandler>>();
            _mockCardRepository = new Mock<ICardRepository>();
            _mockCurrencyConversionService = new Mock<ICurrencyConversionService>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CardProfile());
            }, new NullLoggerFactory());
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task GetCardBalanceSuccessfully()
        {
            //Arrange
            var data = new Card
            {
                Id = 1,
                CardNumber = "1234567890123456",
                CreditLimit = 100,
                Transactions = new List<PurchaseTransaction>()
                {
                    new PurchaseTransaction
                    {
                        Id = 1,
                        Description = "Test desc",
                        TransactionDate = new DateTime(2026, 01, 17),
                        Amount = 10,
                        CardId = 1
                    }
                }
            };

            var id = 1;
            var currency = "Algeria-Dinar";
            _mockCardRepository.Setup(x => x.GetCardByIdWithTransactionsAsync(id)).ReturnsAsync(data);

            //Act
            var sut = new GetCardBalanceQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mockCurrencyConversionService.Object, _mapper);
            var result = await sut.Handle(new GetCardBalanceQuery(id, currency), new CancellationToken());

            //Assert
            _mockCardRepository.Verify(x => x.GetCardByIdWithTransactionsAsync(id), Times.Once);
            result.Id.Should().Be(1);
            result.CardNumber.Should().Be("1234567890123456");
            result.AvailableBalanceInUsd.Should().Be(90);
        }
    }
}