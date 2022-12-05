using Daily_Exchange_Rates.ViewModels;
using Daily_Exchange_Rates.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Daily_Exchange_Rates
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }

    }
}
