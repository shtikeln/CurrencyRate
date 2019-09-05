using CurrencyRate.Api.Responses;
using CurrencyRate.Api.Serializers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Clients
{
    public class BankGovUaClient : IBankGovUaClient
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonSerializer _serializer;

        public BankGovUaClient(HttpClient httpClient, IJsonSerializer serializer)
        {
            _httpClient = httpClient;
            _serializer = serializer;
        }

        public async Task<List<BankGovUaRateResponse>> Exchange(DateTime date)
        {
            string _url = "statdirectory/exchange";

            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
                {
                    { "date", date.ToString("yyyyMMdd") },
                    { "json", "" }
                };
            string uri = QueryHelpers.AddQueryString(_url, queryParameters);

            using (HttpResponseMessage response = await _httpClient.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return _serializer.Deserialize<List<BankGovUaRateResponse>>(content);
                }

                response.EnsureSuccessStatusCode();
                return null;
            }
        }
    }
}
