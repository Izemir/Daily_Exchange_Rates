using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Daily_Exchange_Rates.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "О приложении";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/Izemir/Daily_Exchange_Rates"));
        }

        public ICommand OpenWebCommand { get; }
    }
}