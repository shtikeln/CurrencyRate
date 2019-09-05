using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CurrencyRate.Api.Responses
{
    [XmlRoot("rates")]
    public class NbRatesResponse
    {
        [XmlElement("generator")]
        public string Generator { get; set; }
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("link")]
        public string Link { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("copyright")]
        public string Copyright { get; set; }
        [XmlElement("date")]
        public string Date { get; set; }
        [XmlElement("item")]
        public List<NbItemResponse> Items { get; set; } = new List<NbItemResponse>();
        [XmlElement("info")]
        public string Info { get; set; }
    }
}
