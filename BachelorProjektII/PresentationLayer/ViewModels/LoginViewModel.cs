using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using Android.App;
using Android.Content.PM;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using BachelorProjectII.DomainLayer.Services;
using BachelorProjectII.DomainLayer.Models;
using DevExpress.Maui.Core.Internal;
using Microsoft.Maui.Graphics.Text;
using System.Net.Http.Headers;
using BachelorProjectII.PresentationLayer.Views;

namespace BachelorProjectII.PresentationLayer.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IMitIdLoginService _mitIdLoginService;

        public LoginViewModel(IMitIdLoginService mitIdLoginService)
        {
            _mitIdLoginService = mitIdLoginService;
        }

        public async Task<bool> Login()
        {
            return await _mitIdLoginService.LoginWithMitId();
        }
    }
}
