using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentCard.API.AutoMapper;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Application.Interfaces.Services;
using PaymentCard.Application.QueryHandlers;
using PaymentCard.Domain;
using static PaymentCard.Application.Queries.Queries;

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
            result.AvailableBalanceInUsd.Should().Be(90m);
            // conversion service was not setup to return values so fields remain null
            result.ExchangeRateUsed.Should().BeNull();
            result.AvailableBalanceConverted.Should().BeNull();
        }

        [Fact]
        public async Task GetCardBalanceWithoutCurrency_DoesNotCallConversionService()
        {
            // Arrange
            var data = new Card
            {
                Id = 2,
                CardNumber = "2222222222222222",
                CreditLimit = 200m,
                Transactions = new List<PurchaseTransaction>()
                {
                    new PurchaseTransaction { Id = 1, Amount = 50m, TransactionDate = DateTime.Now, Description = "T1", CardId = 2 }
                }
            };

            var id = 2;
            string? currency = null;

            _mockCardRepository.Setup(x => x.GetCardByIdWithTransactionsAsync(id)).ReturnsAsync(data);

            // Act
            var sut = new GetCardBalanceQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mockCurrencyConversionService.Object, _mapper);
            var result = await sut.Handle(new GetCardBalanceQuery(id, currency), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.GetCardByIdWithTransactionsAsync(id), Times.Once);
            _mockCurrencyConversionService.Verify(x => x.ConvertAmountToCurrencyAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Never);
            result.AvailableBalanceInUsd.Should().Be(150m);
            result.AvailableBalanceConverted.Should().BeNull();
            result.ExchangeRateUsed.Should().BeNull();
        }

        [Fact]
        public async Task GetCardBalanceWithCurrency_UsesConversionServiceAndPopulatesConvertedFields()
        {
            // Arrange
            var data = new Card
            {
                Id = 3,
                CardNumber = "3333333333333333",
                CreditLimit = 500m,
                Transactions = new List<PurchaseTransaction>()
                {
                    new PurchaseTransaction { Id = 1, Amount = 100m, TransactionDate = DateTime.Now, Description = "T1", CardId = 3 },
                    new PurchaseTransaction { Id = 2, Amount = 50m, TransactionDate = DateTime.Now, Description = "T2", CardId = 3 }
                }
            };

            var id = 3;
            var currency = "Algeria-Dinar";

            _mockCardRepository.Setup(x => x.GetCardByIdWithTransactionsAsync(id)).ReturnsAsync(data);

            var availableBalance = data.CreditLimit - data.Transactions.Sum(t => t.Amount); // 350
            var expectedRate = 140.0m;
            var expectedConverted = Math.Round(availableBalance * expectedRate, 2, MidpointRounding.AwayFromZero); // 49000.00m

            _mockCurrencyConversionService
                .Setup(x => x.ConvertAmountToCurrencyAsync(
                    It.Is<string>(s => s == currency),
                    It.Is<decimal>(d => d == availableBalance),
                    It.IsAny<DateTime>()))
                .ReturnsAsync((expectedRate, expectedConverted, currency.ToUpperInvariant(), (string?)null));

            // Act
            var sut = new GetCardBalanceQueryHandler(_mockLogger.Object, _mockCardRepository.Object, _mockCurrencyConversionService.Object, _mapper);
            var result = await sut.Handle(new GetCardBalanceQuery(id, currency), CancellationToken.None);

            // Assert
            _mockCardRepository.Verify(x => x.GetCardByIdWithTransactionsAsync(id), Times.Once);
            _mockCurrencyConversionService.Verify(x => x.ConvertAmountToCurrencyAsync(currency, availableBalance, It.IsAny<DateTime>()), Times.Once);

            result.AvailableBalanceInUsd.Should().Be(availableBalance);
            result.ExchangeRateUsed.Should().Be(expectedRate);
            result.AvailableBalanceConverted.Should().Be(expectedConverted);
            result.TargetCurrency.Should().Be(currency.ToUpperInvariant());
            result.ErrorMessage.Should().BeNull();
        }
    }
}