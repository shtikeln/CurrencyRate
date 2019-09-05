using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Services
{
    public abstract class GenericCurrencyRateService : ICurrencyRateService
    {
        public virtual async Task<decimal> ConvertSum(decimal sum, DateTime date)
        {
            ValidateSum(sum);
            ValidateDate(date);
            decimal rate = await GetRate(date);
            return rate * sum;
        }

        public virtual async Task<decimal> GetRate(DateTime date)
        {
            throw new NotImplementedException("Method 'GetRate' should be implemented in inheritied classes.");
            //return await Task.Run(() => { return 0.0M; }); 
        }

        protected bool ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(date), $"Parameter '{nameof(date)}' cannot be from the future. Parameter value: '{date}'");
            }
            return true;
        }

        protected bool ValidateSum(decimal sum)
        {
            if (sum <= 0)
            {
                throw new ArgumentException($"Parameter '{nameof(sum)}' should be greater than zero. Parameter value: '{sum}'", nameof(sum));
            }
            return true;
        }
    }
}
