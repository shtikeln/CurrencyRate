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
            BankGovUaRateResponse item = response.First(x => x.Cc == "RUB");
            return item.Rate;
        }
    }
}
