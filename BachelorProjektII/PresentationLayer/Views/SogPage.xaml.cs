namespace BachelorProjectII.PresentationLayer.Views;

using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.PresentationLayer.ViewModels;

public partial class SogPage : ContentPage
{
    private SogViewModel _SogViewModel;
    public SogPage(SogViewModel sogViewModel)
	{
		InitializeComponent();
        _SogViewModel = sogViewModel;
        BindingContext = _SogViewModel;
        
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _SogViewModel.OnAppearing();
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchBar = sender as SearchBar;
        SearchResultLabel.Text = $"Du søgte efter indlægssedlen: {searchBar.Text}";
        _SogViewModel.OnSearch(searchBar.Text);
    }

    // Når teksten i søgefeltet ændres
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        // Tjek om teksten er blevet ryddet (dvs. den er tom)
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            // Udfør den ønskede handling her, når "X" er trykket, og teksten er ryddet
            SearchResultLabel.Text = "Indlægsedlen vises her...";
            _SogViewModel.ClearSearchResults(); 
        }
        else
        {
            // Opdater tekst i realtid eller filtrer indhold baseret på tekst
            SearchResultLabel.Text = $"Søger: {e.NewTextValue}";
        }
    }

    private void OnScanButtonClicked(object sender, EventArgs e)
    {
        // Her skal der være kode for at åbne kamera og scanne streg-kode)
        DisplayAlert("Scan", "Scan-funktionen er ikke implementeret endnu.", "OK");
    }

}