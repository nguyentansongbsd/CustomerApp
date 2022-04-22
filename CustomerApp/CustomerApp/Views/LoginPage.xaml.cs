using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LoginViewModel();
        }

        private void LabelField_ValueTapped(object sender, EventArgs e)
        {
            textfield.Text = "aaaaaaa";
        }

        private async void ButtonCustom_Clicked(object sender, EventArgs e)
        {
            btn.isBusy = true;
            await Task.Delay(3000);
            btn.isBusy = false;
        }

        private void ButtonCustom_Clicked_1(object sender, EventArgs e)
        {
           string a = viewModel.Text;
        }
    }
}
