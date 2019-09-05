using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyRate.Api.Responses
{
    public class BankGovUaRateResponse
    {
        [JsonProperty("r030")]
        public int R030 { get; set; }

        [JsonProperty("txt")]
        public string Txt { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("cc")]
        public string Cc { get; set; }

        [JsonProperty("exchangedate")]
        public string ExchangeDateString { get; set; }

        [JsonIgnore]
        public DateTime ExchangeDate { get => DateTime.ParseExact(ExchangeDateString, "dd.MM.yyyy", null); }
    }
}
