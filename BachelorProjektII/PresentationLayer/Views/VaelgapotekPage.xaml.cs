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
        SearchResultLabel.Text = $"Du s�gte efter apoteket: {searchBar.Text}";
        _vaelgapotekViewModel.OnSearch(searchBar.Text);
    }

    // N�r teksten i s�gefeltet �ndres
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        // Opdater tekst i realtid eller filtrer indhold baseret p� tekst
        SearchResultLabel.Text = $"S�ger: {e.NewTextValue}";
    }

    //n�r der trykkes p� knappen "skift apotek" �ndres fortrukne apotek
    private void OnSkiftValgtApotekButtonClicked(object sender, EventArgs e)
    {
        _vaelgapotekViewModel.ChangePreferredApotek();
    }
}