using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CurrencyRate.Api.Serializers
{
    public class JsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
