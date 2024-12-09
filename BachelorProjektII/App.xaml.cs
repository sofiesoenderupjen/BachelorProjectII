using Android.SE.Omapi;
using BachelorProjectII.DataLayer;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using BachelorProjectII.PresentationLayer.ViewModels;
using BachelorProjectII.PresentationLayer.Views;
using Javax.Security.Auth.Login;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;

namespace BachelorProjectII
{
    public partial class App : Application
    {
        private readonly PersonService _personService;

        public App(PersonService personService)
        {
            InitializeComponent();
            _personService = personService;

            // Set an initial blank page
            MainPage = new ContentPage();

            // Check if user is logged in and navigate accordingly
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            var user = await Task.Run(() => _personService.LoadPersonFromPreferences());

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (user == null)
                {
                    // Navigate to login page if the user is not logged in
                    MainPage = new MainPage();
                    await Shell.Current.GoToAsync("//loginPage");
                }
                else
                {
                    // Navigate to PIN login page if the user exists but needs to confirm
                    MainPage = new MainPage();
                    await Shell.Current.GoToAsync("//pinkodeLoginPage");
                }
            });
        }
    }

}
