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
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
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
    }
}
