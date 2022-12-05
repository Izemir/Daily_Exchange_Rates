using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Essentials;

namespace Daily_Exchange_Rates.Services.CurrencyService
{
    public class CurrencyService : ICurrencyService
    {
        public async Task<IEnumerable<CurrencyData>> GetActualCurrencyAsync()
        {
            var result = new List<CurrencyData>();
            List<Currency> first = new List<Currency>();
            List<Currency> second = new List<Currency>();
            first = GetCurrencyFromWeb(DateTime.Now.AddDays(1));
            if (first != null)
            {
                second = GetCurrencyFromWeb(DateTime.Now);
                Preferences.Set("date", "tomorrow");
            }
            else
            {
                Preferences.Set("date", "today");
                first = GetCurrencyFromWeb(DateTime.Now);
                second = GetCurrencyFromWeb(DateTime.Now.AddDays(-1));
            }
            if(first!=null&& second!=null)
            {
                result = MergeLists(first, second);
            }

            return await Task.FromResult(result);
        }

        private List<CurrencyData> MergeLists(List<Currency> first, List<Currency> second)
        {
            var result = new List<CurrencyData>();
            try
            {
                
                foreach(var item in first) 
                {                    
                    var secondRate = second.First(i=>i.CharCode==item.CharCode).Rate;
                    result.Add(new CurrencyData()
                    {
                        CharCode= item.CharCode,
                        Name = item.Name,
                        NumCode = item.NumCode,
                        Rate= item.Rate,
                        Scale = item.Scale,
                        PreviousRate = secondRate
                    });
                }

                return result;
            }
            catch(Exception ex)
            {
                return result;
            }
        }

        private static T Deserialize<T>(string data) where T : class, new()
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var ser = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(data))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        private List<Currency> GetCurrencyFromWeb(DateTime date)
        {
            var result = new List<Currency>();
            try
            {
                var request = WebRequest.Create($"https://www.nbrb.by/Services/XmlExRates.aspx?ondate={date.ToString("dd.MM.yyyy")}") as HttpWebRequest;
                var response = request.GetResponse();

                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                var xml = readStream.ReadToEnd();

                var currencyList = XDocument.Parse(xml)
                           .Descendants("Currency")
                           .ToList();

                if (currencyList.Count == 0) return null;

                foreach (var currency in currencyList)
                {
                    var item = Deserialize<Currency>(currency.ToString());
                    result.Add(item);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

            return result;
        }
    }
}
