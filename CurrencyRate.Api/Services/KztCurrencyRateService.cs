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
            if (response == null)
                throw new Exception("Нет данных на указанную дату");
            NbItemResponse nbItem = response.Items.FirstOrDefault(x => x.Title == "RUB");
            if (nbItem == null)
                throw new Exception("Нет курса на указанную дату");
            return nbItem.Description / nbItem.Quant;
        }
    }
}
