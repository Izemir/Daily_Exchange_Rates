﻿using Daily_Exchange_Rates.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Daily_Exchange_Rates.Models.Rates;

namespace Daily_Exchange_Rates.Services.CurrencyService
{
    public class APICurrencyService :CurrencyService, ICurrencyService
    {
        private string _dateFormat = "yyyy-MM-dd";
        private readonly HttpClient _http;
        private readonly string _apiConnection;
        public APICurrencyService()
        {
            _http = new HttpClient();
            _apiConnection = "https://www.nbrb.by/api/exrates/rates?periodicity=0";
        }

        protected override async Task<List<Currency>> GetCurrencyFromWeb(DateTime date)
        {
            var result = new List<Currency>();
            try
            {
                var request = await _http.GetAsync($"{_apiConnection}&ondate={date.ToString(_dateFormat)}");
                if(!request.IsSuccessStatusCode) return result;
                var response = await request.Content.ReadFromJsonAsync<List<Rate>>();

                if (response == null) return result;
                if(response.Count == 0) return result;

                result = RatesMapper(response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return result;
            }

            return result;
        }

        private List<Currency> RatesMapper(List<Rate> rates)
        {
            var result = new List<Currency>();
            foreach (var rate in rates)
            {
                result.Add(new Currency
                {
                    CharCode=rate.Cur_Abbreviation,
                    Name=rate.Cur_Name,
                    Rate= (double)(rate.Cur_OfficialRate??0),
                    Scale=rate.Cur_Scale,
                });
            }
            return result;
        }
    }


    
}