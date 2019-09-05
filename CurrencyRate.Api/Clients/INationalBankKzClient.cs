using System;
using System.Threading.Tasks;
using CurrencyRate.Api.Responses;

namespace CurrencyRate.Api.Clients
{
    public interface INationalBankKzClient
    {
        Task<NbRatesResponse> GetRates(DateTime date);
    }
}