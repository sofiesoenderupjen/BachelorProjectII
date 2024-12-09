using DevExpress.Maui;
using DevExpress.Maui.Core;
using BachelorProjectII.PresentationLayer.Views;
using BachelorProjectII.DataLayer.Repositories;
using BachelorProjectII.DomainLayer.Services;
using BachelorProjectII.PresentationLayer.ViewModels;
using BachelorProjectII.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BachelorProjectII
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ThemeManager.ApplyThemeToSystemBars = true;

            var builder = MauiApp.CreateBuilder();

            builder.Services.AddSingleton<DBConnect>();
            builder.Services.AddSingleton<IIndlaegsseddelRepository, IndlaegsseddelRepository>();
            builder.Services.AddSingleton<IIndlaegsseddelService, IndlaegsseddelService>();
            builder.Services.AddSingleton<IApotekRepository, ApotekRepository>();
            builder.Services.AddSingleton<IApotekService, ApotekService>();
            builder.Services.AddSingleton<IPersonRepository, PersonRepository>();
            builder.Services.AddSingleton<IPersonService, PersonService>();
            builder.Services.AddSingleton<IMitIdLoginService, MitIdLoginService>();
            builder.Services.AddSingleton<PersonService>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<PinkodeLoginPage>();
            builder.Services.AddTransient<PinkodeLoginViewModel>();
            builder.Services.AddTransient<MineIndlaegssedlerViewModel>();            
            builder.Services.AddTransient<MineIndlaegssedlerPage>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MinProfilViewModel>();
            builder.Services.AddTransient<MinProfilPage>(); 
            builder.Services.AddTransient<MerePage>();
            builder.Services.AddTransient<MereViewModel>();
            builder.Services.AddSingleton<DetaljerViewModel>();
            builder.Services.AddSingleton<DetaljerPage>();
            builder.Services.AddSingleton<App>();
            builder.Services.AddTransient<SogPage>();
            builder.Services.AddTransient<SogViewModel>();
            builder.Services.AddTransient<DitvalgteapotekPage>();
            builder.Services.AddTransient<DitvalgteapotekViewModel>();

            builder.Services.AddTransient<VaelgapotekPage>();
            builder.Services.AddTransient<VaelgapotekViewModel>();

            builder
                .UseMauiApp<App>()
                .UseDevExpress(useLocalization: true)
                .UseDevExpressControls()
                .UseDevExpressCharts()
                .UseDevExpressCollectionView()
                .UseDevExpressEditors()
                .UseDevExpressDataGrid()
                .UseDevExpressScheduler()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("roboto-regular.ttf", "Roboto");
                    fonts.AddFont("roboto-medium.ttf", "Roboto-Medium");
                    fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                });
            DevExpress.Maui.Charts.Initializer.Init();
            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();
            DevExpress.Maui.DataGrid.Initializer.Init();
            DevExpress.Maui.Scheduler.Initializer.Init();

            return builder.Build();
        }
    }
}