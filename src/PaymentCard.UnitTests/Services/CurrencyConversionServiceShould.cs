using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentCard.Application.Interfaces.Services;
using PaymentCard.Application.Services;
using PaymentCard.Application.Services.models;
using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.UnitTests.Services
{
    public class CurrencyConversionServiceShould
    {
        private static Mock<ILogger<CurrencyConversionService>> _mockLogger = null!;
        private static Mock<ICurrencyService> _mockCurrencyService = null!;

        public CurrencyConversionServiceShould()
        {
            _mockLogger = new Mock<ILogger<CurrencyConversionService>>();
            _mockCurrencyService = new Mock<ICurrencyService>();
        }

        [Fact]
        public async Task ConvertTransactionsToCurrencySuccessfully()
        {
            //Arrange
            var data = new List<TreasuryExchangeRateDto>
            {
                new TreasuryExchangeRateDto
                {
                    RecordDate = DateTime.Now.AddDays(-2),
                    Country = "Algeria",
                    Currency = "Dinar",
                    CountryCurrencyDescription = "Algeria-Dinar",
                    ExchangeRate = 140.0m,
                    EffectiveDate = DateTime.Now.AddDays(-2),
                    SourceLineNumber = 1
                }
            };

            var transactionData = new List<PurchaseTransaction>
            {
                new PurchaseTransaction
                {
                    Id = 1,
                    CardId = 1,
                    Amount = 14000m,
                    TransactionDate = DateTime.Now,
                    Description = "Test Transaction"
                }
            };

            _mockCurrencyService.Setup(x => x.GetExchangeRatesAsync()).ReturnsAsync(data);

            //Act
            var sut = new CurrencyConversionService(_mockLogger.Object, _mockCurrencyService.Object);
            var result = await sut.ConvertTransactionsToCurrencyAsync("Algeria-Dinar", transactionData);

            //Assert
            _mockCurrencyService.Verify(x => x.GetExchangeRatesAsync(), Times.Once);
            result.exchangeRateUsed.Should().Be(140.0m);
            result.convertedAmount.Should().Be(1960000m);
            result.targetCurrency.Should().Be("ALGERIA-DINAR");
            result.errorMessage.Should().BeNull();
        }

        [Fact]
        public async Task ConvertAmountToCurrencySuccessfully()
        {
            //Arrange
            var data = new List<TreasuryExchangeRateDto>
            {
                new TreasuryExchangeRateDto
                {
                    RecordDate = DateTime.Now.AddDays(-2),
                    Country = "Algeria",
                    Currency = "Dinar",
                    CountryCurrencyDescription = "Algeria-Dinar",
                    ExchangeRate = 140.0m,
                    EffectiveDate = DateTime.Now.AddDays(-2),
                    SourceLineNumber = 1
                }
            };

            _mockCurrencyService.Setup(x => x.GetExchangeRatesAsync()).ReturnsAsync(data);

            //Act
            var sut = new CurrencyConversionService(_mockLogger.Object, _mockCurrencyService.Object);
            var result = await sut.ConvertAmountToCurrencyAsync("Algeria-Dinar", 14000m, DateTime.Now);

            //Assert
            _mockCurrencyService.Verify(x => x.GetExchangeRatesAsync(), Times.Once);
            result.exchangeRateUsed.Should().Be(140.0m);
            result.convertedAmount.Should().Be(1960000m);
            result.targetCurrency.Should().Be("ALGERIA-DINAR");
            result.errorMessage.Should().BeNull();
        }
    }
}