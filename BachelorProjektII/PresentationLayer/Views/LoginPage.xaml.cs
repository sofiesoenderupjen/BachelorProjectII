using Android.App;
using Android.Content.PM;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using BachelorProjectII.PresentationLayer.ViewModels;
using BachelorProjectII.DomainLayer.Services;

namespace BachelorProjectII.PresentationLayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _loginViewModel;

        public LoginPage(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            _loginViewModel = loginViewModel;
            BindingContext = _loginViewModel;
            
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var login = await Task.Run(() => _loginViewModel.Login());
            if (!login)
            {
                await DisplayAlert("Login med MitID", "Login med MitID mislykkes - Prøv igen.", "OK");
            }
        }
    }
}      
    
