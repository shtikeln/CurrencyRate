using CurrencyRate.Api.Clients;
using CurrencyRate.Api.Responses;
using CurrencyRate.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CurrencyRate.Api.Test
{
    public class UahCurrencyRateServiceTests
    {
        [Fact]
        public void GetRate_DateFromFuture_ExceptionThrown()
        {
            var client = new Mock<IBankGovUaClient>();
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetRate(DateTime.MaxValue));
        }

        [Theory]
        [MemberData(nameof(CorrectData))]
        public async void GetRate_CorrectInput_ReturnResult(DateTime date, List<BankGovUaRateResponse> response)
        {
            var client = new Mock<IBankGovUaClient>();
            client.Setup(p => p.Exchange(It.IsAny<DateTime>())).ReturnsAsync(response);
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            decimal expected = 0.02534M;
            decimal actual = await service.GetRate(date);

            Assert.Equal(expected, actual, 5);
        }

        [Fact]
        public void GetRate_NullResponse_ExceptionThrown()
        {
            var client = new Mock<IBankGovUaClient>();
            List<BankGovUaRateResponse> response = null;
            client.Setup(p => p.Exchange(It.IsAny<DateTime>())).ReturnsAsync(response);
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            Assert.ThrowsAsync<Exception>(() => service.GetRate(DateTime.Now));
        }

        [Fact]
        public void GetRate_EmptyResponse_ExceptionThrown()
        {
            var client = new Mock<IBankGovUaClient>();
            List<BankGovUaRateResponse> response = new List<BankGovUaRateResponse>();
            client.Setup(p => p.Exchange(It.IsAny<DateTime>())).ReturnsAsync(response);
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            Assert.ThrowsAsync<Exception>(() => service.GetRate(DateTime.Now));
        }

        [Fact]
        public void ConvertSum_DateFromFuture_ExceptionThrown()
        {
            var client = new Mock<IBankGovUaClient>();
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.ConvertSum(10, DateTime.MaxValue));
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-10.0)]
        public void ConvertSum_SumIsLessOrEqualZero_ExceptionThrown(decimal sum)
        {
            var client = new Mock<IBankGovUaClient>();
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.ConvertSum(sum, DateTime.Now));
        }

        [Theory]
        [MemberData(nameof(CorrectData))]
        public async void ConvertSum_CorrectInput_ReturnResult(DateTime date, List<BankGovUaRateResponse> response)
        {
            var client = new Mock<IBankGovUaClient>();
            client.Setup(p => p.Exchange(It.IsAny<DateTime>())).ReturnsAsync(response);
            UahCurrencyRateService service = new UahCurrencyRateService(client.Object);

            decimal expected = 25.34M;
            decimal actual = await service.ConvertSum(1000, date);

            Assert.Equal(expected, actual, 3);
        }

        public static IEnumerable<object[]> CorrectData()
        {
            List<BankGovUaRateResponse> result = new List<BankGovUaRateResponse>()
            { new BankGovUaRateResponse() { R030 = 1, Cc = "RUB", ExchangeDateString = "20.08.2019", Rate = 0.02534M, Txt = "Рубль" } };

            return new List<object[]>
            {
                new object[] { DateTime.Now, result },
                new object[] { DateTime.Now.AddDays(-7), result },
                new object[] { DateTime.Now.AddMonths(-5), result }
            };
        }
    }
}
