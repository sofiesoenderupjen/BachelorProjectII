using BachelorProjectII.DataLayer;
using BachelorProjectII.PresentationLayer.ViewModels;
using System.Runtime.CompilerServices;

namespace BachelorProjectII.PresentationLayer.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
            
            //routing sker her fordi "mainPage" arver fra "shell", og de nedensteånde skal ikke være en del af "tab-bar":
            Routing.RegisterRoute("vaelgapotekPage", typeof(VaelgapotekPage));
            Routing.RegisterRoute("minProfilPage", typeof(MinProfilPage));
            Routing.RegisterRoute("detaljerPage", typeof(DetaljerPage));
        }

    }
}