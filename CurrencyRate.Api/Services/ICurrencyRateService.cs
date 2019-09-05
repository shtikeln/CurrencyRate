using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Services
{
    public interface ICurrencyRateService
    {
        Task<decimal> GetRate(DateTime date);

        Task<decimal> ConvertSum(decimal sum, DateTime date);
    }
}
