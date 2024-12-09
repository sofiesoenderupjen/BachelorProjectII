using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class VaelgapotekViewModel : BaseViewModel
    {
        private readonly IApotekService _apotekService;
        private readonly IPersonService _personService;

        private PersonModel _currentPerson {  get; set; }   
        
        public ICommand ItemSelectedCommand { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand ReturnCommand => new Command(() => MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync("//ditvaelgteapotekPage"); }));


        public ObservableCollection<ApotekModel> _filteredapoteker { get; set; }
        public ObservableCollection<ApotekModel> FilteredApoteker
        {
            get => _filteredapoteker;
            set
            {
                _filteredapoteker = value;
                OnPropertyChanged(nameof(FilteredApoteker));
            }
        }
        
        private ApotekModel _selectedApotek;
        public ApotekModel SelectedApotek
        {
            get => _selectedApotek;
            set
            {
                _selectedApotek = value;
                OnPropertyChanged(nameof(SelectedApotek));
            }
        }

        public ObservableCollection<ApotekModel> Apoteker { get; set; }

        public bool SkiftApotekButtonEnabled { get; set; }

        public VaelgapotekViewModel(IApotekService apotekService, IPersonService personService)
        {
            _apotekService = apotekService;
            _personService = personService;

            Apoteker = new ObservableCollection<ApotekModel>();

            FilteredApoteker = new ObservableCollection<ApotekModel>(Apoteker);
            SearchCommand = new Command<string>(OnSearch);
            ItemSelectedCommand = new Command<ApotekModel>(OnItemSelected);
            SkiftApotekButtonEnabled = false;
        }

        public void OnSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Hvis der ikke er nogen søgning, vis ikke nogen inlægssedel
                FilteredApoteker = new ObservableCollection<ApotekModel>();
            }
            else
            {
                // Filtrer efter Navn eller Virksomhed
                var filteredResults = Apoteker
                    .Where(i => i.ApotekNavn.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                i.Adresse.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                FilteredApoteker = new ObservableCollection<ApotekModel>(filteredResults);
            }

            SkiftApotekButtonEnabled = false;
            OnPropertyChanged(nameof(SkiftApotekButtonEnabled));
        }

        public async void OnAppearing()
        {
            SkiftApotekButtonEnabled = false;
            OnPropertyChanged(nameof(SkiftApotekButtonEnabled));
            Apoteker.Clear();
            FilteredApoteker.Clear();

            _currentPerson = await _personService.LoadPersonFromPreferences();

            //hent data fra servicelaget
            var apotekerFromService = await _apotekService.GetApoteker();
            if (apotekerFromService != null)
            {
                if (_currentPerson.FortrukneApotekId != 0)
                {
                    var nuvaerendeApotek = apotekerFromService.FirstOrDefault(a => a.Id == _currentPerson.FortrukneApotekId); // Find det ønskede objekt

                    // Hvis elementet findes i listen, så flyt det til første position
                    if (nuvaerendeApotek != null)
                    {
                        apotekerFromService.Remove(nuvaerendeApotek); // Fjern elementet
                        apotekerFromService.Insert(0, nuvaerendeApotek); // Tilføj elementet som det første i listen
                    }
                    nuvaerendeApotek.IsSelected = true;
                }
                Apoteker = new ObservableCollection<ApotekModel>(apotekerFromService);
                FilteredApoteker = Apoteker;
            }
        }
       
        private void OnItemSelected(ApotekModel selectedApotek)
        {
            // løkken her gennemgår alle apoteker i filteredapoteker, og sætter til false, så der kun er ét spotek der kan være markeret ad gangen
            foreach (var apotek in FilteredApoteker)
            {
                apotek.IsSelected = false;
            }
            //denne metode giver besked opm at filtered apoteker-egenskaben er blevet opdateret
            OnPropertyChanged(nameof(FilteredApoteker));

            // Sæt det valgte apotek til true(valgt)
            selectedApotek.IsSelected = true;
            //opdaterer valgte apotek, og aktiverer knap:
            SelectedApotek = selectedApotek;
            SkiftApotekButtonEnabled = true;
            OnPropertyChanged(nameof(SkiftApotekButtonEnabled));
        }

        //her ændres foretrukne apotek, og når der trykkes på knappen, går den til "ditvalgte apotek" siden
        public async void ChangePreferredApotek()
        {
            if (SelectedApotek != null)
            {
                if (await _personService.SetFortrukneApotek(_currentPerson.Id, SelectedApotek.Id))
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync("//ditvaelgteapotekPage");
                    });
                }
            }
        }
    }
}
