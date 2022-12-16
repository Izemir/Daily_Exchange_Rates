using Daily_Exchange_Rates.Models;
using Daily_Exchange_Rates.Services;
using Daily_Exchange_Rates.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Forms.Device;

namespace Daily_Exchange_Rates.ViewModels
{    
    /// <summary>
    /// Настройка списка валют
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        public CurrencySetting itemBeingDragged;
        public ObservableCollection<CurrencySetting> Settings { get;}
        public Command<CurrencySetting> DragStartingCommand { get; }
        public Command<CurrencySetting> DragOverCommand { get; }


        public Command SaveCommand { get; }

        private SettingService _settingService;

        /// <summary>
        /// Первая загрузка окна и первые необходимые запуска методов.
        /// </summary>
        public SettingsViewModel() 
        {
            //Title = "Настройка валют";
            Settings = new ObservableCollection<CurrencySetting>();
            SaveCommand = new Command(Save);
            _settingService = new SettingService();

            // фиксируется перетаскиваемая "валюта"
            DragStartingCommand = new Command<CurrencySetting>((s) =>
            {
                itemBeingDragged = s;
            });

            DragOverCommand = new Command<CurrencySetting>((s) => MoveItem(s));
            LoadSettings();
        }

        /// <summary>
        /// Перетаскиваемая "валюта" меняется местами с той, на которой остановилась
        /// </summary>
        /// <param name="s">"Валюта" остановки</param>
        private void MoveItem(CurrencySetting s)
        {
            if (s.CharCode == itemBeingDragged.CharCode)
                return;
            int oldIndex = s.Order;
            s.Order = itemBeingDragged.Order;
            itemBeingDragged.Order = oldIndex;
            Settings.Move(Settings.IndexOf(itemBeingDragged), Settings.IndexOf(s));
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
            _settingService.SaveSettings(Settings.ToList());
            await Shell.Current.GoToAsync("..");
        }

    }
}
