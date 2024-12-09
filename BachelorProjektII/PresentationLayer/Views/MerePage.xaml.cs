using BachelorProjectII.PresentationLayer.ViewModels;

namespace BachelorProjectII.PresentationLayer.Views;

public partial class MerePage : ContentPage
{
    private MereViewModel _mereViewModel;
    public MerePage(MereViewModel mereViewModel)
    {
        InitializeComponent();
        _mereViewModel = mereViewModel;
        BindingContext = _mereViewModel;
        Title = "Mere";
    }

    private void OnMinProfilButtonClicked(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("minProfilPage");
        });
    }

    private void OnIndstillingerButtonClicked(object sender, EventArgs e)
    {
        //Ikke implementeret
        //MainThread.BeginInvokeOnMainThread(async () =>
        //{
        //    await Shell.Current.GoToAsync("//indstillingerPage");
        //});
    }

    private void OnLogUdButtonClicked(object sender, EventArgs e)
    {
        _mereViewModel.LogUd();
    }

}