﻿using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daily_Exchange_Rates.Services.CurrencyService
{
    public class MockCurrencyService : ICurrencyService
    {
        List<Currency> _currencies;
        public MockCurrencyService()
        {
            _currencies= new List<Currency>()
            {
                new Currency()
                {
                    Id=440,
                    NumCode=036,
                    CharCode="AUD",
                    Scale=1,
                    Name="Австралийский доллар",
                    Rate=2.1052,
                    PreviousRate=2.1052
                },
                new Currency()
                {
                    Id=510,
                    NumCode=051,
                    CharCode="AMD",
                    Scale=1000,
                    Name="Армянских драмов",
                    Rate=5.9545,
                    PreviousRate = 5.9545
                },
            };
        }
        public async Task<IEnumerable<Currency>> GetActualCurrencyAsync()
        {            
            return await Task.FromResult(_currencies);
        }
    }
}