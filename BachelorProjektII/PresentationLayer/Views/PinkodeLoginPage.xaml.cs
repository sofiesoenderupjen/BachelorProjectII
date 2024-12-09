using AndroidX.Lifecycle;
using BachelorProjectII.PresentationLayer.ViewModels;

namespace BachelorProjectII.PresentationLayer.Views;

public partial class PinkodeLoginPage : ContentPage
{
    private readonly PinkodeLoginViewModel _pinkodeLoginViewModel;

    public PinkodeLoginPage(PinkodeLoginViewModel pinkodeLoginViewModel)
	{
		InitializeComponent();
        _pinkodeLoginViewModel = pinkodeLoginViewModel;
        BindingContext = _pinkodeLoginViewModel;

        if (_pinkodeLoginViewModel != null)
        {
            _pinkodeLoginViewModel.PinConfirmationRequested += OnPinConfirmationRequested;
            _pinkodeLoginViewModel.PinEntryResetRequested += OnPinEntryResetRequested;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _pinkodeLoginViewModel.OnAppearing();
    }

    private void OnPinBoxCompleted(object sender, EventArgs e)
    {
        if (sender == PinBox1 && !string.IsNullOrEmpty(PinBox1.Text))
            PinBox2.Focus();
        else if (sender == PinBox2 && !string.IsNullOrEmpty(PinBox2.Text))
            PinBox3.Focus();
        else if (sender == PinBox3 && !string.IsNullOrEmpty(PinBox3.Text))
            PinBox4.Focus();
        else if (sender == PinBox4 && !string.IsNullOrEmpty(PinBox4.Text))
            PinBox4.Unfocus(); 
    }
    private void OnPinEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length == 1) 
        {
            if (sender == PinBox1)
                PinBox2.Focus();
            else if (sender == PinBox2)
                PinBox3.Focus();
            else if (sender == PinBox3)
                PinBox4.Focus();
            else if (sender == PinBox4)
            {
                PinBox4.Unfocus(); 
            }
        }
        else if (e.NewTextValue.Length == 0) 
        {
            if (sender == PinBox4)
            {
                PinBox3.Focus();
            }
            else if (sender == PinBox3)
            {
                PinBox2.Focus();
            }
            else if (sender == PinBox2)
            {
                PinBox1.Focus();
            }
        }
    }
    private void OnPinConfirmationRequested(object sender, EventArgs e)
    {
        PinBox1.Text = PinBox2.Text = PinBox3.Text = PinBox4.Text = string.Empty;
        PinBox1.Focus(); 
    }

    private void OnPinEntryResetRequested(object sender, EventArgs e)
    {
        PinBox1.Text = PinBox2.Text = PinBox3.Text = PinBox4.Text = string.Empty;
        PinBox1.Focus();
    }
}