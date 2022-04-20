using System;
using System.Collections.Generic;
using CustomerApp.ViewModels;
using CustomerApp.Views;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

