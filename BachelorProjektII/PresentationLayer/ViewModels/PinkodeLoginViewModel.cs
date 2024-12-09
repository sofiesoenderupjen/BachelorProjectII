using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class PinkodeLoginViewModel : BaseViewModel
    {
        private readonly IPersonService _personService;
        private string _pin1;
        private string _pin2;
        private string _pin3;
        private string _pin4;
        private string _message;
        private string _confirmButtonText;
        private bool _isConfirmingPin;
        private string _firstEnteredPin;
        private bool _isNewUser;

        public event EventHandler PinConfirmationRequested;
        public event EventHandler PinEntryResetRequested;

        public PinkodeLoginViewModel(IPersonService personService)
        {
            _personService = personService;
            ConfirmCommand = new Command(CompletePinEntry);
        }

        public string Pin1
        {
            get => _pin1;
            set
            {
                _pin1 = value;
                OnPropertyChanged();
            }
        }

        public string Pin2
        {
            get => _pin2;
            set
            {
                _pin2 = value;
                OnPropertyChanged();
            }
        }

        public string Pin3
        {
            get => _pin3;
            set
            {
                _pin3 = value;
                OnPropertyChanged();
            }
        }

        public string Pin4
        {
            get => _pin4;
            set
            {
                _pin4 = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmButtonText
        {
            get => _confirmButtonText;
            set
            {
                _confirmButtonText = value;
                OnPropertyChanged();
            }
        }

        public bool IsNewUser
        {
            get => _isNewUser;
            set
            {
                _isNewUser = value;
                OnPropertyChanged();
            }
        }

        private PersonModel _currentPerson;
        public PersonModel CurrentPerson
        {
            get => _currentPerson;
            set => SetProperty(ref _currentPerson, value);
        }

        public ICommand ConfirmCommand { get; }


        public async void OnAppearing()
        {
            Pin1 = "";
            Pin2 = "";
            Pin3 = "";
            Pin4 = "";

            CurrentPerson = await _personService.LoadPersonFromPreferences();
            IsNewUser = CurrentPerson.Pinkode == 0; // Assume 0 PIN means a new user

            if (IsNewUser)
            {
                Message = "Indtast din nye pinkode to gange";
                ConfirmButtonText = "Bekræft";
            }
            else
            {
                Message = "Indtast din pinkode for at logge ind";
                ConfirmButtonText = "Login";
            }
        }

        public void CompletePinEntry()
        {
            if (IsNewUser)
            {
                if (!_isConfirmingPin)
                {
                    // First PIN entry completed, start confirmation step
                    _isConfirmingPin = true;
                    _firstEnteredPin = $"{Pin1}{Pin2}{Pin3}{Pin4}";
                    Message = "Bekræft din nye pinkode";

                    // Trigger the event to clear entries and refocus for confirmation
                    PinConfirmationRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    // Confirmation step: compare entered PINs
                    string confirmationPin = $"{Pin1}{Pin2}{Pin3}{Pin4}";
                    if (_firstEnteredPin == confirmationPin)
                    {
                        SavePin(confirmationPin);
                    }
                    else
                    {
                        Message = "Pinkoderne stemmer ikke overens. Prøv igen.";
                        _isConfirmingPin = false;
                        PinEntryResetRequested?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                // Existing user: Validate entered PIN
                ValidateExistingUserPin();
            }
        }

        private async void SavePin(string pin)
        {

            bool success = await _personService.UpdatePinkode(CurrentPerson.Id, int.Parse(pin));
            if (success)
            {
                CurrentPerson.Pinkode = int.Parse(pin);
                // Navigate to main page
                _personService.LogIn(CurrentPerson);
                await _personService.AddRandomReceptIndlaegssedlerForPersonAsync(CurrentPerson.UUID);
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//forsidePage");
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fejl", "Kunne ikke sætte pinkode. Prøv igen.", "OK");
            }
        }

        private void ValidateExistingUserPin()
        {
            string enteredPin = $"{Pin1}{Pin2}{Pin3}{Pin4}";
            if (CurrentPerson.Pinkode.ToString().Equals(enteredPin, StringComparison.OrdinalIgnoreCase))
            {
                _personService.LogIn(CurrentPerson);
                // Navigate to main page
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//forsidePage");
                });
            }
            else
            {
                Message = "Forkert pinkode. Prøv igen.";
                PinEntryResetRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
