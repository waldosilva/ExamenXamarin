using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ExamenXamarin.Models;
using ExamenXamarin.ViewModels;

namespace ExamenXamarin.Views
{
    public partial class NewItemPage : ContentPage
    {
    //public Item Item { get; set; }
    public Usuario Usuario { get; set; }

    public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
