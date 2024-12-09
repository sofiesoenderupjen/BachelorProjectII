using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.PresentationLayer.ViewModels;

namespace BachelorProjectII.PresentationLayer.Views;

public partial class DetaljerPage : ContentPage
{
    private readonly DetaljerViewModel _detaljerViewModel;

    public DetaljerPage(DetaljerViewModel detaljerViewModel)
	{		
        InitializeComponent();
        _detaljerViewModel = detaljerViewModel;
        BindingContext = _detaljerViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _detaljerViewModel.OnAppearing();
    }

    private void IndlaegsseddelButton_Clicked(object sender, EventArgs e)
    {
        _detaljerViewModel.ShowIndlaegsseddelHeaders();
    }

    private void LagerstatusButton_Clicked(object sender, EventArgs e)
    {
        _detaljerViewModel.ShowLagerstatus();
    }
}