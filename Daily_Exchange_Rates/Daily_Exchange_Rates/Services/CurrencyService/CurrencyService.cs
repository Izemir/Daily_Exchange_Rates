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
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Daily_Exchange_Rates.Services.CurrencyService
{
    public class CurrencyService : ICurrencyService
    {

        private string _dateFormat = "MM.dd.yyyy";
        /// <summary>
        /// Получает данные на завтра и сегодня, если их нет, то на вчера-сегодня.
        /// После чего идет приведение и соединение в один список.
        /// После чего идет фильтрация через настройки.
        /// </summary>
        /// <returns>Возвращает данные в нужном виде (списком)</returns>
        public async Task<IEnumerable<CurrencyData>> GetActualCurrencyAsync()
        {
            var result = new List<CurrencyData>();
            List<Currency> first = new List<Currency>();
            List<Currency> second = new List<Currency>();
            first = await GetCurrencyFromWeb(DateTime.Now.AddDays(1));
            if (first.Count>0)
            {
                second = await GetCurrencyFromWeb(DateTime.Now);
                Preferences.Set("date", "tomorrow");
            }
            else
            {
                Preferences.Set("date", "today");
                first = await GetCurrencyFromWeb(DateTime.Now);
                second = await GetCurrencyFromWeb(DateTime.Now.AddDays(-1));
            }
            if (first.Count > 0 && second.Count > 0)
            {
                result = MergeLists(first, second);
                SettingService settingService = new SettingService();
                settingService.AdaptCurrencyList(ref result);
            }
            else return null;

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Соединяет полученные данные за 2 дня в один список, и маппит их
        /// </summary>
        /// <param name="first">Первый список</param>
        /// <param name="second">Второй список</param>
        /// <returns>Соединенный список</returns>
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
                        ScaleName = item.Scale+" "+item.Name,
                        NumCode = item.NumCode,
                        Rate= item.Rate,
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

        /// <summary>
        /// Десериализация xml-документа по одному объекту
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Получение данных с сайта по дате, обработка их, десериализация
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Список данных с сайта</returns>
        protected virtual Task<List<Currency>> GetCurrencyFromWeb(DateTime date)
        {
            var result = new List<Currency>();
            try
            {
                var request = WebRequest.Create($"https://www.nbrb.by/Services/XmlExRates.aspx?ondate={date.ToString(_dateFormat)}") as HttpWebRequest;
                var response = request.GetResponse();

                if (response == null) return Task.FromResult(result);

                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                var xml = readStream.ReadToEnd();

                var currencyList = XDocument.Parse(xml)
                           .Descendants("Currency")
                           .ToList();

                if (currencyList.Count == 0) return Task.FromResult(result);

                foreach (var currency in currencyList)
                {
                    var item = Deserialize<Currency>(currency.ToString());
                    result.Add(item);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }
    }
}
