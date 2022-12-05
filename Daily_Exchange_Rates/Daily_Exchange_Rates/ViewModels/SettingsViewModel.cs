using Daily_Exchange_Rates.Models;
using Daily_Exchange_Rates.Services;
using Daily_Exchange_Rates.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Daily_Exchange_Rates.ViewModels
{
    /// <summary>
    /// Настройка списка валют
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        public List<CurrencySetting> Settings { get;}

        public Command SaveCommand { get; }

        private SettingService _settingService;

        /// <summary>
        /// Первая загрузка окна и первые необходимые запуска методов.
        /// </summary>
        public SettingsViewModel() 
        {
            Title = "Настройка валют";
            Settings = new List<CurrencySetting>();
            SaveCommand = new Command(Save);
            _settingService = new SettingService();
            LoadSettings();
        }

        /// <summary>
        /// Загрузка списка настроек
        /// </summary>
        private void LoadSettings()
        {
            var data = _settingService.GetSettings();
            if(data != null)
            {
                foreach(var item in data)
                {
                    Settings.Add(item);
                }
            }
        }

        /// <summary>
        /// Сохранение настроек и возвращение на предыдущее окно
        /// </summary>
        /// <param name="obj"></param>
        private async void Save(object obj)
        {
            _settingService.SaveSettings(Settings);
            await Shell.Current.GoToAsync("..");
        }

    }
}
