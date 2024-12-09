using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Java.Util;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class MineIndlaegssedlerViewModel : BaseViewModel
    {
        private readonly PersonService _personService;

        public ICommand ItemSelectedCommand { get; }
        
        public MineIndlaegssedlerViewModel(PersonService personService) 
        {
            _personService = personService;

            GemteIndlaegssedler = new ObservableCollection<IndlaegsseddelModel>();
            ReceptIndlaegssedler = new ObservableCollection<IndlaegsseddelModel>();

            ItemSelectedCommand = new Command<IndlaegsseddelModel>(OnItemSelected);
        }

        public ObservableCollection<IndlaegsseddelModel> GemteIndlaegssedler { get; set; }
        public ObservableCollection<IndlaegsseddelModel> ReceptIndlaegssedler { get; set; }

        async public void OnAppearing()
        {
            GemteIndlaegssedler.Clear();
            ReceptIndlaegssedler.Clear();
            var _currentPerson = await _personService.LoadPersonFromPreferences();

            if (_currentPerson.ReceptIndlaegssedler != null)
            {
                foreach (var indlaegsseddel in _currentPerson.ReceptIndlaegssedler)
                {
                    ReceptIndlaegssedler.Add(indlaegsseddel);
                }
            }

            if (_currentPerson.GemteIndlaegssedler != null)
            {
                foreach (var indlaegsseddel in _currentPerson.GemteIndlaegssedler)
                {
                    GemteIndlaegssedler.Add(indlaegsseddel);
                }
            }
        }

        private void OnItemSelected(IndlaegsseddelModel selectedIndlaegsseddel)
        {
            if (selectedIndlaegsseddel != null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("detaljerPage", new Dictionary<string, object>
                    {
                        { "SelectedIndlaegsseddel", selectedIndlaegsseddel },
                        { "ReturnToMineIndlaegssedler", true }
                    });
                });
            }
        }
    }
}
