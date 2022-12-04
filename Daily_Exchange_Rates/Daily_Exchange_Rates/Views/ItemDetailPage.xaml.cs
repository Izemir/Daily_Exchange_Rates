using Daily_Exchange_Rates.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Daily_Exchange_Rates.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}