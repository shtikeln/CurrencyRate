using CurrencyRate.Api.Clients;
using CurrencyRate.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Services
{
    public class UahCurrencyRateService : GenericCurrencyRateService
    {
        private readonly IBankGovUaClient _client;

        public UahCurrencyRateService(IBankGovUaClient client)
        {
            _client = client;
        }

        public override async Task<decimal> GetRate(DateTime date)
        {
            ValidateDate(date);
            List<BankGovUaRateResponse> response = await _client.Exchange(date);
            if (response == null)
                throw new Exception("Нет данных на указанную дату");
            BankGovUaRateResponse item = response.FirstOrDefault(x => x.Cc == "RUB");
            if (item == null)
                throw new Exception("Нет курса на указанную дату");
            return item.Rate;
        }
    }
}
