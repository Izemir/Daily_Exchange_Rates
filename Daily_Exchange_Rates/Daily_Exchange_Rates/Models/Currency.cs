using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Exchange_Rates.Models
{
    /// <summary>
    /// Модель данных, получаемых с сайта
    /// </summary>
    public class Currency
    {
        public int NumCode { get; set; }
        public string CharCode { get; set; }
        public int Scale { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
}
