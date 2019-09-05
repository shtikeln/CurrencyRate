using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CurrencyRate.Api.Serializers
{
    public class XmlSerializer : IXmlSerializer
    {
        public T Deserialize<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(stream);
            }
        }
    }
}
