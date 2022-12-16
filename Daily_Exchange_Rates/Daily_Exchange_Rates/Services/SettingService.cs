using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Essentials;

namespace Daily_Exchange_Rates.Services
{
    /// <summary>
    /// Сервис сохранения и применения настроек
    /// </summary>
    public class SettingService
    {
        private List<CurrencySetting> _settings;
        private string[] _defaultSettings;
        private const string _keyString = "settings";

        /// <summary>
        /// Настройки десереализуются из xml-документа, сохраненного в приложении.
        /// Определяется список валют "по умолчанию".
        /// </summary>
        public SettingService()
        {
            _defaultSettings = new string[] { "USD", "EUR", "RUB" };
            _settings = new List<CurrencySetting>();
            if (Preferences.ContainsKey(_keyString))
            {
                var savedData = Preferences.Get(_keyString, "");
                if(!string.IsNullOrEmpty(savedData)) 
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CurrencySetting>));

                    using (StringReader textReader = new StringReader(savedData))
                    {
                        List<CurrencySetting> settings = xmlSerializer.Deserialize(textReader) as List<CurrencySetting>;
                        if (settings != null)
                        {
                            if(settings.Count>0) _settings= settings;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получение настроек
        /// </summary>
        /// <returns>Список настроек</returns>
        public List<CurrencySetting> GetSettings()
        {
            return _settings;
        }

        /// <summary>
        /// Изменение данных (видимость в приложении и порядок) согласно настройкам.
        /// Если настроек нет, применяются стандартные и сохраняется весь список возможной валюты.
        /// </summary>
        /// <param name="list"></param>
        public void AdaptCurrencyList(ref List<CurrencyData> list)
        {
            var newSettings = new List<CurrencySetting>();
            foreach(var item in list)
            {
                if (_settings.Count > 0)
                {
                    var tmpItem = _settings.FirstOrDefault(i=>i.CharCode==item.CharCode);
                    if(tmpItem != null)
                    {
                        item.IsVisible = tmpItem.Enable;
                    }
                    else
                    {
                        item.IsVisible = false;
                    }
                }
                else
                {
                    if(_defaultSettings.Contains(item.CharCode))
                    {
                        item.IsVisible= true;
                    }
                    else item.IsVisible= false;

                    newSettings.Add(new CurrencySetting()
                    {
                        CharCode= item.CharCode,
                        Enable=item.IsVisible,
                        Order=newSettings.Count+1,
                        ScaleName=item.ScaleName,
                    });                    
                }
            }
            if (newSettings.Count > 0) SaveSettings(newSettings);
        }

        /// <summary>
        /// Сохранение настроек в виде xml-документа в самом приложении
        /// </summary>
        /// <param name="settings">Список настроек</param>
        public void SaveSettings(List<CurrencySetting> settings)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CurrencySetting>));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, settings);
                    Preferences.Set(_keyString, textWriter.ToString());
                }
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
            }
            
        }
    }
}
