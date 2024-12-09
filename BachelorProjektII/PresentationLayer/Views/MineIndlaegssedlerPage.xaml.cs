using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using BachelorProjectII.PresentationLayer.ViewModels;
using DevExpress.Xpo.DB;

namespace BachelorProjectII.PresentationLayer.Views;

public partial class MineIndlaegssedlerPage : ContentPage
{
	private MineIndlaegssedlerViewModel _mineIndlaegsseddelViewModel;
	public MineIndlaegssedlerPage(MineIndlaegssedlerViewModel mineIndlaegssedlerViewModel)
	{
		InitializeComponent();
		_mineIndlaegsseddelViewModel = mineIndlaegssedlerViewModel;
		BindingContext = _mineIndlaegsseddelViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _mineIndlaegsseddelViewModel.OnAppearing();
    }
}
