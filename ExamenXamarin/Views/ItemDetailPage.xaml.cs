using System.ComponentModel;
using Xamarin.Forms;
using ExamenXamarin.ViewModels;

namespace ExamenXamarin.Views
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