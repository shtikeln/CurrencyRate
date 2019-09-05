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
            throw new NotImplementedException("Метод 'GetRate' должен быть реализован в наследуемом классе.");
        }

        protected bool ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(date), $"Параметр '{nameof(date)}' не должен быть больше текущей даты. Значение параметра: '{date}'");
            }
            return true;
        }

        protected bool ValidateSum(decimal sum)
        {
            if (sum <= 0)
            {
                throw new ArgumentException($"Параметр '{nameof(sum)}' должен быть больше нуля. Значение параметра: '{sum}'", nameof(sum));
            }
            return true;
        }
    }
}
