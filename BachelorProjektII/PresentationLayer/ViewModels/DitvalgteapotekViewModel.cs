using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BachelorProjectII.PresentationLayer.ViewModels
{

    public class DitvalgteapotekViewModel : BaseViewModel
    {
        private readonly IApotekService _apotekService;
        private readonly PersonService _personService;

        private ApotekModel _valgteApotek;
        public ApotekModel ValgteApotek
        {
            get => _valgteApotek;
            set
            {
                _valgteApotek = value;
                OnPropertyChanged();
            }
        }
        public DitvalgteapotekViewModel(PersonService personService, IApotekService apotekService)
        {
            _apotekService = apotekService;
            _personService = personService;
        }


        async public void OnAppearing()
        {
            ValgteApotek = null;
            var currentPerson = await _personService.LoadPersonFromPreferences();
            if (currentPerson.FortrukneApotekId == 0 || currentPerson.FortrukneApotekId == null)
            {
                await Shell.Current.GoToAsync("vaelgapotekPage");
            }
            else
            {
                ValgteApotek = await _apotekService.GetApotekById(currentPerson.FortrukneApotekId.Value);
            }
        }
    }
}