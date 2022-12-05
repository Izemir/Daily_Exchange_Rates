using Daily_Exchange_Rates.Models;
using Daily_Exchange_Rates.Services.CurrencyService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Daily_Exchange_Rates.ViewModels
{
    public class CurrencyListViewModel: BaseViewModel
    {
        public List<CurrencyData> Currency { get; }

        public string FirstDate { get; set; }
        public string SecondDate { get; set; }

        public Command LoadCurrencyCommand { get; }
        public CurrencyListViewModel()
        {
            Title = "Курсы валют";
            Currency= new List<CurrencyData>();
            LoadCurrencyCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            //MockCurrencyService mockCurrencyService = new MockCurrencyService();
            CurrencyService currencyService = new CurrencyService();
            try
            {
                Currency.Clear();
                var currency = await currencyService.GetActualCurrencyAsync();
                foreach (var item in currency)
                {
                    Currency.Add(item);
                }
                if (Preferences.ContainsKey("date"))
                {
                    string date = Preferences.Get("date","");
                    if(date == "today")
                    {
                        FirstDate = DateTime.Now.AddDays(-1).ToString("dd.MM.yy");
                        SecondDate = DateTime.Now.ToString("dd.MM.yy");
                    }
                    else if(date == "tomorrow")
                    {
                        FirstDate = DateTime.Now.ToString("dd.MM.yy");
                        SecondDate = DateTime.Now.AddDays(1).ToString("dd.MM.yy");
                    }
                }
                else
                {
                    FirstDate = DateTime.Now.ToString("dd.MM.yy");
                    SecondDate = DateTime.Now.AddDays(-1).ToString("dd.MM.yy");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
