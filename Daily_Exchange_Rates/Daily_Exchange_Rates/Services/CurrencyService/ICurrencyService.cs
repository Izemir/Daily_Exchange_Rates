using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Exchange_Rates.Services.CurrencyService
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Получение данных с сайта (можно было бы разделить на 2 метода - 
        /// получение данных на сегодня-завтра и вчера-сегодня
        /// </summary>
        /// <returns>Возвращает данные в нужном виде (списком)</returns>
        Task<IEnumerable<CurrencyData>> GetActualCurrencyAsync();
    }
}
