using BachelorProjectII.DomainLayer.Models;
using BachelorProjectII.DomainLayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class MereViewModel : BaseViewModel
    {
        private readonly IPersonService _personService;
        
        public MereViewModel(IPersonService personService)
        {
            _personService = personService;
        }

        public void LogUd()
        {
            _personService.LogOut();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//loginPage");
            });
        }
    }
}
