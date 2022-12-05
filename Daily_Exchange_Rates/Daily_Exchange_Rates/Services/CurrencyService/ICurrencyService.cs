using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Exchange_Rates.Services.CurrencyService
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyData>> GetActualCurrencyAsync();
    }
}
