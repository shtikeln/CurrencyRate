using CurrencyRate.Api.Clients;
using CurrencyRate.Api.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Services
{
    public class KztCurrencyRateService : GenericCurrencyRateService
    {
        private readonly INationalBankKzClient _client;

        public KztCurrencyRateService(INationalBankKzClient client)
        {
            _client = client;
        }

        public override async Task<decimal> GetRate(DateTime date)
        {
            ValidateDate(date);
            NbRatesResponse response = await _client.GetRates(date);
            NbItemResponse nbItem = response.Items.First(x => x.Title == "RUB");
            return nbItem.Description / nbItem.Quant;
        }
    }
}
