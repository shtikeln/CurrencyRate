using CurrencyRate.Api.Responses;
using CurrencyRate.Api.Serializers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CurrencyRate.Api.Clients
{
    public class NationalBankKzClient : INationalBankKzClient
    {
        private readonly HttpClient _httpClient;
        private readonly IXmlSerializer _serializer;

        public NationalBankKzClient(HttpClient httpClient, IXmlSerializer serializer)
        {
            _httpClient = httpClient;
            _serializer = serializer;
        }

        public async Task<NbRatesResponse> GetRates(DateTime date)
        {
            string _url = "get_rates.cfm";        

            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
                {
                    { "fdate", date.ToString("dd.MM.yyyy") }
                };

            string uri = QueryHelpers.AddQueryString(_url, queryParameters);

            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return _serializer.Deserialize<NbRatesResponse>(content);
            }

            response.EnsureSuccessStatusCode();
            return null;
        }
    }
}
