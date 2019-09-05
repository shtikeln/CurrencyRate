using CurrencyRate.Api.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Clients
{
    public interface IBankGovUaClient
    {
        Task<List<BankGovUaRateResponse>> Exchange(DateTime date);
    }
}