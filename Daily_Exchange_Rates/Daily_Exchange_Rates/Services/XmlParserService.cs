using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Daily_Exchange_Rates.Services
{
    public class XmlParserService
    {
        XmlSerializer xmlSerializer;
        public XmlParserService() 
        {
            xmlSerializer = new XmlSerializer(typeof(List<Currency>));
        }
        public List<Currency> ParseCurrencyFromFile(string path) 
        {
            var result = new List<Currency>();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, result);
            }
            return result;
        }
    }
}
