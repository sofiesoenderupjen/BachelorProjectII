using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class MinProfilViewModel : BaseViewModel
    {
        private readonly IPersonService _personService;
        public ICommand ReturnCommand => new Command(() => MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync("//merePage"); }));

        public MinProfilViewModel(IPersonService personService)
        {
            _personService = personService;
        }

        private PersonModel _currentPerson;
        public PersonModel CurrentPerson
        {
            get => _currentPerson;
            set => SetProperty(ref _currentPerson, value);
        }

        public async void OnAppearing()
        {
            CurrentPerson = await Task.Run(() => _personService.LoadPersonFromPreferences());
        }
    }
}

