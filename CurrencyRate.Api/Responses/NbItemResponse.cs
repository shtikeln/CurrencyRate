using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CurrencyRate.Api.Responses
{
    public class NbItemResponse
    {
        [XmlElement("fullname")]
        public string Fullname { get; set; }
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("description")]
        public decimal Description { get; set; }
        [XmlElement("quant")]
        public int Quant { get; set; }
        [XmlElement("index")]
        public string Index { get; set; }
        [XmlElement("change")]
        public decimal Change { get; set; }
    }
}
