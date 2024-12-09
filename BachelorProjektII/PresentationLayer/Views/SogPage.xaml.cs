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
        SearchResultLabel.Text = $"Du s�gte efter indl�gssedlen: {searchBar.Text}";
        _SogViewModel.OnSearch(searchBar.Text);
    }

    // N�r teksten i s�gefeltet �ndres
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        // Tjek om teksten er blevet ryddet (dvs. den er tom)
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            // Udf�r den �nskede handling her, n�r "X" er trykket, og teksten er ryddet
            SearchResultLabel.Text = "Indl�gsedlen vises her...";
            _SogViewModel.ClearSearchResults(); 
        }
        else
        {
            // Opdater tekst i realtid eller filtrer indhold baseret p� tekst
            SearchResultLabel.Text = $"S�ger: {e.NewTextValue}";
        }
    }

    private void OnScanButtonClicked(object sender, EventArgs e)
    {
        // Her skal der v�re kode for at �bne kamera og scanne streg-kode)
        DisplayAlert("Scan", "Scan-funktionen er ikke implementeret endnu.", "OK");
    }

}