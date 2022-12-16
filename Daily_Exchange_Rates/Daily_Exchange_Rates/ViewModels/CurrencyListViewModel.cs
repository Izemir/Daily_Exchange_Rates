using Daily_Exchange_Rates.Models;
using Daily_Exchange_Rates.Services.CurrencyService;
using Daily_Exchange_Rates.Views;
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
    /// <summary>
    /// Отображение списка валюты и курса
    /// </summary>
    public class CurrencyListViewModel: BaseViewModel
    {

        private string _dateFormat = "dd.MM.yy";
        public ObservableCollection<CurrencyData> Currency { get; }

        public Command SettingsCommand { get; }

        private bool _error;
        private bool _noError;
        public bool Error
        {
            get
            {
                return _error;
            }
            set
            {
                SetProperty(ref _error, value);
                NoError = !_error;
            }
        }

        public bool NoError
        {
            get
            {
                return _noError;
            }
            set
            {
                SetProperty(ref _noError, value);
            }
        }

        private string _firstDate;
        private string _secondDate;
        public string FirstDate {
            get
            {
                return _firstDate;
            }
            set
            {
                SetProperty(ref _firstDate, value);
            }
        }
        public string SecondDate
        {
            get
            {
                return _secondDate;
            }
            set
            {
                SetProperty(ref _secondDate, value);
            }
        }

        public string ErrorText { get; set; }

        public Command LoadCurrencyCommand { get; }

        /// <summary>
        /// Первая загрузка окна и первые необходимые запуска методов.
        /// </summary>
        public CurrencyListViewModel()
        {
            Title = "Курсы валют";
            Currency= new ObservableCollection<CurrencyData>();
            LoadCurrencyCommand = new Command(async () => await ExecuteLoadCommand());
            Error= false;
            ErrorText = "Не удалось получить курсы валют";
            SettingsCommand = new Command(Settings);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        /// <summary>
        /// Метод для обновления данных с сайта (работает при загрузке страницы и ее обновлении).
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            try
            {
                Currency.Clear();
                var currency = await CurrencyService.GetActualCurrencyAsync();
                if (currency != null)
                {
                    foreach (var item in currency)
                    {
                        if(item.IsVisible) Currency.Add(item);
                    }
                    if (Preferences.ContainsKey("date")) // задается дата в заголовке в зависимости от полученных данных
                    {
                        string date = Preferences.Get("date", "");
                        if (date == "today")
                        {
                            FirstDate = DateTime.Now.AddDays(-1).ToString(_dateFormat);
                            SecondDate = DateTime.Now.ToString(_dateFormat);
                        }
                        else if (date == "tomorrow")
                        {
                            FirstDate = DateTime.Now.ToString(_dateFormat);
                            SecondDate = DateTime.Now.AddDays(1).ToString(_dateFormat);
                        }
                    }
                    else
                    {
                        FirstDate = DateTime.Now.AddDays(-1).ToString(_dateFormat);
                        SecondDate = DateTime.Now.ToString(_dateFormat);
                    }
                    Error= false;
                }
                else
                {
                    Error= true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Error= true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Переход на страницу настроек
        /// </summary>
        /// <param name="obj"></param>
        private async void Settings(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }


    }
}
