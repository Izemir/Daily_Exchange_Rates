using Daily_Exchange_Rates.Resx;
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
            //Title = "О приложении";
            var link = AppResources.GithubLink;
            OpenWebCommand = new Command(async () => await Browser.OpenAsync(link));
        }

        public ICommand OpenWebCommand { get; }
    }
}