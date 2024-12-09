using BachelorProjectII.PresentationLayer.ViewModels;

namespace BachelorProjectII.PresentationLayer.Views;

public partial class MinProfilPage : ContentPage
{
    private MinProfilViewModel _minProfilViewModel;

	public MinProfilPage(MinProfilViewModel minProfilViewModel)
	{        
        InitializeComponent();
		_minProfilViewModel = minProfilViewModel;
        BindingContext = _minProfilViewModel;        

	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _minProfilViewModel.OnAppearing();
    }

}