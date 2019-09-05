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
    public class KztCurrencyRateServiceTests
    {
        [Fact]
        public void GetRate_DateFromFuture_ExceptionThrown()
        {
            var client = new Mock<INationalBankKzClient>();
            KztCurrencyRateService service = new KztCurrencyRateService(client.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetRate(DateTime.MaxValue));
        }

        [Theory]
        [MemberData(nameof(CorrectData))]
        public async void GetRate_CorrectInput_ReturnResult(DateTime date, NbRatesResponse response)
        {
            var client = new Mock<INationalBankKzClient>();
            client.Setup(p => p.GetRates(It.IsAny<DateTime>())).ReturnsAsync(response);
            KztCurrencyRateService service = new KztCurrencyRateService(client.Object);

            decimal expected = 5.8M;
            decimal actual = await service.GetRate(date);

            Assert.Equal(expected, actual, 1);
        }

        [Fact]
        public void ConvertSum_DateFromFuture_ExceptionThrown()
        {
            var client = new Mock<INationalBankKzClient>();
            KztCurrencyRateService service = new KztCurrencyRateService(client.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.ConvertSum(10, DateTime.MaxValue));
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-10.0)]
        public void ConvertSum_SumIsLessOrEqualZero_ExceptionThrown(decimal sum)
        {
            var client = new Mock<INationalBankKzClient>();
            KztCurrencyRateService service = new KztCurrencyRateService(client.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.ConvertSum(sum, DateTime.Now));
        }

        [Theory]
        [MemberData(nameof(CorrectData))]
        public async void ConvertSum_CorrectInput_ReturnResult(DateTime date, NbRatesResponse response)
        {
            var client = new Mock<INationalBankKzClient>();
            client.Setup(p => p.GetRates(It.IsAny<DateTime>())).ReturnsAsync(response);
            KztCurrencyRateService service = new KztCurrencyRateService(client.Object);

            decimal expected = 87;
            decimal actual = await service.ConvertSum(15, date);

            Assert.Equal(expected, actual, 1);
        }

        public static IEnumerable<object[]> CorrectData()
        {
            NbRatesResponse result = new NbRatesResponse()
            { Items = new List<NbItemResponse>() { new NbItemResponse() { Title = "RUB", Description = 58, Quant = 10 } } };

            return new List<object[]>
            {
                new object[] { DateTime.Now, result },
                new object[] { DateTime.Now.AddDays(-7), result },
                new object[] { DateTime.Now.AddMonths(-5), result }
            };
        }

    }
}
