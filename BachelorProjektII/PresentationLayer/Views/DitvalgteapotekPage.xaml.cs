using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.PresentationLayer.ViewModels;

namespace BachelorProjectII.PresentationLayer.Views;

public partial class DitvalgteapotekPage : ContentPage
{
    private DitvalgteapotekViewModel _ditvalgteapotekViewModel;
    public DitvalgteapotekPage(DitvalgteapotekViewModel ditvalgteapotekViewModel)
	{
        InitializeComponent();
        _ditvalgteapotekViewModel = ditvalgteapotekViewModel;
        BindingContext = _ditvalgteapotekViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ditvalgteapotekViewModel.OnAppearing();
    }

    private async void OnSkiftApotekButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("vaelgapotekPage");
    }
}