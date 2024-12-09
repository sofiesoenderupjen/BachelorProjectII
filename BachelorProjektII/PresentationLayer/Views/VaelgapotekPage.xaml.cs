using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.PresentationLayer.ViewModels;

namespace BachelorProjectII.PresentationLayer.Views;
public partial class VaelgapotekPage : ContentPage
{
    private VaelgapotekViewModel _vaelgapotekViewModel;
    public VaelgapotekPage(VaelgapotekViewModel vaelgapotekViewModel)
    {
        InitializeComponent();
        _vaelgapotekViewModel = vaelgapotekViewModel;
        BindingContext = _vaelgapotekViewModel;
        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vaelgapotekViewModel.OnAppearing();
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchBar = sender as SearchBar;
        SearchResultLabel.Text = $"Du søgte efter apoteket: {searchBar.Text}";
        _vaelgapotekViewModel.OnSearch(searchBar.Text);
    }

    // Når teksten i søgefeltet ændres
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        // Opdater tekst i realtid eller filtrer indhold baseret på tekst
        SearchResultLabel.Text = $"Søger: {e.NewTextValue}";
    }

    //når der trykkes på knappen "skift apotek" ændres fortrukne apotek
    private void OnSkiftValgtApotekButtonClicked(object sender, EventArgs e)
    {
        _vaelgapotekViewModel.ChangePreferredApotek();
    }
}