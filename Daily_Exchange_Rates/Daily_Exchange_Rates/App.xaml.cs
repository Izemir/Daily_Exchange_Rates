using Daily_Exchange_Rates.Services;
using Daily_Exchange_Rates.Services.CurrencyService;
using Daily_Exchange_Rates.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Daily_Exchange_Rates
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<CurrencyService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
