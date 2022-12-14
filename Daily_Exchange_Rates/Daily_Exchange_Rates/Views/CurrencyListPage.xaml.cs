using Daily_Exchange_Rates.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Daily_Exchange_Rates.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyListPage : ContentPage
    {
        CurrencyListViewModel _viewModel;
        public CurrencyListPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CurrencyListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}