using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Exchange_Rates.Models
{
    /// <summary>
    /// Модель настроек для хранения видимости валюты и порядка в списке
    /// </summary>
    public class CurrencySetting
    {
        public string CharCode { get; set; }
        public string ScaleName { get; set; }
        public bool Enable { get; set; }
        public int Order { get; set; }
    }
}
