using Daily_Exchange_Rates.Models;
using Daily_Exchange_Rates.Services.CurrencyService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Daily_Exchange_Rates.ViewModels
{
    public class CurrencyListViewModel: BaseViewModel
    {
        public List<Currency> Currency { get; }

        public string FirstDate { get; set; }
        public string SecondDate { get; set; }

        public Command LoadCurrencyCommand { get; }
        public CurrencyListViewModel()
        {
            Title = "Курсы валют";
            Currency= new List<Currency>();
            LoadCurrencyCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            MockCurrencyService mockCurrencyService = new MockCurrencyService();

            try
            {
                Currency.Clear();
                var currency = await mockCurrencyService.GetActualCurrencyAsync();
                foreach (var item in currency)
                {
                    Currency.Add(item);
                }
                FirstDate=DateTime.Now.ToString("dd.MM.yy");
                SecondDate=DateTime.Now.AddDays(-1).ToString("dd.MM.yy"); 
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
