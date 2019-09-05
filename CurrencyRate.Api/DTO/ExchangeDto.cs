using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyRate.Api.Dto
{
    public class ExchangeDto
    {
        public decimal Sum { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyTitle { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Error { get; set; }
    }
}
