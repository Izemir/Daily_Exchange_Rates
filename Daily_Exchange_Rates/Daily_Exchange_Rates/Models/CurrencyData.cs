using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Exchange_Rates.Models
{
    /// <summary>
    /// Модель данных, уже приведенных к необходимому виду для работы приложения
    /// PreviousRate - значения курса на день раньше
    /// IsVisible - видимость в списке
    /// Order - место в списке
    /// </summary>
    public class CurrencyData
    {
        public int Id { get; set; }
        public int NumCode { get; set; }
        public string CharCode { get; set; }
        public string ScaleName { get; set; }
        public double Rate { get; set; }
        public double PreviousRate { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
    }
}
