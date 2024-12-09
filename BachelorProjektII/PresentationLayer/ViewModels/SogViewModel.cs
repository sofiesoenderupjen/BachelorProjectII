using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class SogViewModel : BaseViewModel
    {
        private readonly IIndlaegsseddelService _indlaegsseddelService;
        public ObservableCollection<IndlaegsseddelModel> _filteredindlaegsedler { get; set; }
        public ICommand ItemSelectedCommand { get; }
        public ObservableCollection<IndlaegsseddelModel> FilteredIndlaegssedler
        {
            get => _filteredindlaegsedler;
            set
            {
                _filteredindlaegsedler = value;
                OnPropertyChanged(nameof(FilteredIndlaegssedler));
            }
        }
        
        public ObservableCollection<IndlaegsseddelModel> Indlaegssedler { get; set; }
        public ICommand SearchCommand { get; set; }

        public SogViewModel(IIndlaegsseddelService indlaegsseddelService)
        {
            _indlaegsseddelService = indlaegsseddelService;

            Indlaegssedler = new ObservableCollection<IndlaegsseddelModel>();

            FilteredIndlaegssedler = new ObservableCollection<IndlaegsseddelModel>(Indlaegssedler);
            SearchCommand = new Command<string>(OnSearch);
            ItemSelectedCommand = new Command<IndlaegsseddelModel>(OnItemSelected);
        }
        
        public void OnSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Hvis der ikke er nogen søgning, vis ikke nogen indlægssedel
                FilteredIndlaegssedler = new ObservableCollection<IndlaegsseddelModel>();
            }
            else
            {
                // Filtrer efter Navn eller Virksomhed
                var filteredResults = Indlaegssedler
                    .Where(i => i.Navn.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                i.Virksomhed.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                FilteredIndlaegssedler = new ObservableCollection<IndlaegsseddelModel>(filteredResults);
            }
            OnPropertyChanged(nameof(FilteredIndlaegssedler));
        }

        public void ClearSearchResults()
        {
            FilteredIndlaegssedler.Clear();
            OnPropertyChanged(nameof(FilteredIndlaegssedler));
        }

        public async void OnAppearing()
        {
            Indlaegssedler.Clear();
            //hent data fra servicelaget
            var indlaegssedlerFromService = await _indlaegsseddelService.GetIndlaegssedler();

            if (indlaegssedlerFromService != null)
            {
                Indlaegssedler = new ObservableCollection<IndlaegsseddelModel>(indlaegssedlerFromService);
            }
            OnPropertyChanged(nameof(FilteredIndlaegssedler));
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
                        { "ReturnToMineIndlaegssedler", false }

                    });
                });
            }
        }
    }
}
