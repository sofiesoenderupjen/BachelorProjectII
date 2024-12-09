using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using BachelorProjectII.DomainLayer.Services;
using System.Windows.Input;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    [QueryProperty(nameof(SelectedIndlaegsseddel), "SelectedIndlaegsseddel")]
    [QueryProperty(nameof(ReturnToMineIndlaegssedler), "ReturnToMineIndlaegssedler")]
    public class DetaljerViewModel : BaseViewModel
    {
        private IndlaegsseddelModel _selectedIndlaegsseddel;
        public IndlaegsseddelModel SelectedIndlaegsseddel
        {
            get => _selectedIndlaegsseddel;
            set
            {
                SetProperty(ref _selectedIndlaegsseddel, value);
            }
        }

        private PersonModel _currentPerson;
        public PersonModel CurrentPerson
        {
            get => _currentPerson;
            set
            {
                SetProperty(ref _currentPerson, value);
            }
        }

        private ApotekModel _fortrukneApotek;
        public ApotekModel FortrukneApotek
        {
            get => _fortrukneApotek;
            set
            {
                SetProperty(ref _fortrukneApotek, value);
            }
        }

        private bool _returnToMineIndlaegssedler;
        public bool ReturnToMineIndlaegssedler
        {
            get => _returnToMineIndlaegssedler;
            set
            {
                SetProperty(ref _returnToMineIndlaegssedler, value);
            }
        }

        public bool IndlaegsseddelExpanded { get; set; }

        public bool LagerStatusExpanded { get; set; }
        public bool PaaLager_FortrukneApotek { get; set; }

        public bool GemtIndlaegsseddel => CurrentPerson?.GemteIndlaegssedler?.Any(x=> x.Id == SelectedIndlaegsseddel?.Id) ?? false;

        public bool ReceptIndlaegsseddel => CurrentPerson?.ReceptIndlaegssedler?.Any(x => x.Id == SelectedIndlaegsseddel?.Id) ?? false;

        public string BookmarkIcon
        {
            get
            {
                if (GemtIndlaegsseddel || ReceptIndlaegsseddel)
                    return "bookmark_filled.svg";
                else
                    return "bookmark_empty.svg";
            }
        }

        public ObservableCollection<string> IndlaegsseddelHeaders { get; set; }


        public ObservableCollection<ApotekModel> ApotekerMedIndlaegsseddelPaaLager { get; set; }
        
        public bool ShowFortrukneApotek =>
                    LagerStatusExpanded && FortrukneApotek != null && FortrukneApotek?.Id != 0;

        public ICommand ToggleGemteCommand => new Command(async () => await ToggleGemteIndlaegsseddel());
        public ICommand ReturnCommand => new Command(() => ReturnToPage());


        private readonly PersonService _personService;
        private readonly IApotekService _apotekService;

        public DetaljerViewModel(PersonService personService, IApotekService apotekService)
        {
            _personService = personService;
            _apotekService = apotekService;
            IndlaegsseddelHeaders = new ObservableCollection<string>();
            ApotekerMedIndlaegsseddelPaaLager = new ObservableCollection<ApotekModel>();
            ItemSelectedCommand = new Command<int>(OnItemSelected);

        }

        public async void OnAppearing()
        {
            IndlaegsseddelExpanded = false;
            FortrukneApotek = null;
            LagerStatusExpanded = false;
            IndlaegsseddelHeaders.Clear();
            CurrentPerson = await _personService.LoadPersonFromPreferences();
            PaaLager_FortrukneApotek = true;

            if (CurrentPerson.FortrukneApotekId != 0)
            {
                FortrukneApotek = await _apotekService.GetApotekById(CurrentPerson.FortrukneApotekId.Value);
                PaaLager_FortrukneApotek = await _apotekService.IsIndlaegsseddelPaaLager(FortrukneApotek.Id, SelectedIndlaegsseddel.Id);
            }

            OnPropertyChanged(nameof(CurrentPerson));
            OnPropertyChanged(nameof(ReceptIndlaegsseddel));
            OnPropertyChanged(nameof(GemtIndlaegsseddel));
            OnPropertyChanged(nameof(BookmarkIcon));

            PropertiesChanged();
        }

        public void ReturnToPage()
        {
            if (ReturnToMineIndlaegssedler)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//mineIndlaegssedlerPage");
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//sogPage");
                });
            }
        }

        public ICommand ItemSelectedCommand { get; }

        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => SetProperty(ref selectedIndex, value);
        }

        
        private void OnItemSelected(int index)
        {
            SelectedIndex = index;
        }

        public void ShowIndlaegsseddelHeaders()
        {
            if (!IndlaegsseddelExpanded)
            {
                IndlaegsseddelExpanded = true;
                LagerStatusExpanded = false;
                JsonArray indlaegsseddelArray = JsonSerializer.Deserialize<JsonArray>(SelectedIndlaegsseddel.Indlaegsseddel);

                foreach (var header in indlaegsseddelArray)
                {
                    IndlaegsseddelHeaders.Add(header.ToString());
                }             
            }
            else
            {
                IndlaegsseddelExpanded = false;
                IndlaegsseddelHeaders.Clear(); 
            }
            PropertiesChanged();
        }

        public async void ShowLagerstatus()
        {
            if (!LagerStatusExpanded)
            {
                LagerStatusExpanded = true;
                IndlaegsseddelExpanded = false;
                IndlaegsseddelHeaders.Clear();

                var apoteker = await _apotekService.GetApotekerWithIndlaegsseddelPaaLager(SelectedIndlaegsseddel.Id);
                if (PaaLager_FortrukneApotek)
                {
                    apoteker.Remove(FortrukneApotek);
                }

                if (apoteker != null)
                {
                    if (apoteker.Any())
                    {
                        ApotekerMedIndlaegsseddelPaaLager.Clear();
                        foreach (var apotek in apoteker)
                        {
                            ApotekerMedIndlaegsseddelPaaLager.Add(apotek);
                        }
                    }
                }
            }
            else
            {
                LagerStatusExpanded = false;
                ApotekerMedIndlaegsseddelPaaLager.Clear();
            }
            PropertiesChanged();
        }
        
        public async Task ToggleGemteIndlaegsseddel()
        {
            if (ReceptIndlaegsseddel)
            {
                // Hvis det er i ReceptIndlaegssedler, kan vi ikke fjerne det fra Recept
                return;
            }

            if (GemtIndlaegsseddel)
            {
                // Fjern fra GemteIndlaegssedler
                bool removed = await _personService.RemoveGemteIndlaegsseddel(CurrentPerson.Id, SelectedIndlaegsseddel.Id);
                if (removed)
                {
                    var indlaegsseddelToRemove = CurrentPerson.GemteIndlaegssedler.FirstOrDefault(x => x.Id == SelectedIndlaegsseddel.Id);

                    if (indlaegsseddelToRemove != null)
                    {
                        CurrentPerson.GemteIndlaegssedler.Remove(indlaegsseddelToRemove);
                        OnPropertyChanged(nameof(CurrentPerson.GemteIndlaegssedler));
                        OnPropertyChanged(nameof(CurrentPerson));
                        OnPropertyChanged(nameof(GemtIndlaegsseddel));
                        OnPropertyChanged(nameof(BookmarkIcon));
                    }
                    
                }
            }
            else
            {
                if (CurrentPerson.GemteIndlaegssedler == null)
                {
                    CurrentPerson.GemteIndlaegssedler = new List<IndlaegsseddelModel>();
                }
                // Tilføj til GemteIndlaegssedler
                bool added = await _personService.AddGemteIndlaegsseddel(CurrentPerson.Id, SelectedIndlaegsseddel.Id);
                if (added)
                {
                    CurrentPerson.GemteIndlaegssedler.Add(SelectedIndlaegsseddel);
                    OnPropertyChanged(nameof(CurrentPerson.GemteIndlaegssedler));
                    OnPropertyChanged(nameof(CurrentPerson));
                    OnPropertyChanged(nameof(GemtIndlaegsseddel));
                    OnPropertyChanged(nameof(BookmarkIcon));
                }
            }
        }

        private void PropertiesChanged()
        {
            OnPropertyChanged(nameof(IndlaegsseddelExpanded));
            OnPropertyChanged(nameof(LagerStatusExpanded));
            OnPropertyChanged(nameof(ShowFortrukneApotek));
            OnPropertyChanged(nameof(ApotekerMedIndlaegsseddelPaaLager)); 
            OnPropertyChanged(nameof(PaaLager_FortrukneApotek));
        }
    }
}
