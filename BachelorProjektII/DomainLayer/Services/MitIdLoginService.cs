using Android.App;
using Android.Content.PM;
using BachelorProjectII.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BachelorProjectII.DomainLayer.Services
{
    public class MitIdLoginService : IMitIdLoginService
    {
        public IPersonService _personService;
        public MitIdLoginService(IPersonService personService)
        {
            _personService = personService;
        }

        const string TokenEndpoint = "https://pp.netseidbroker.dk/op/connect/token";
        const string ClientId = "e298b1aa-85ba-4b9c-97c1-977a51e2b215"; 
        const string ClientSecret = "RTd/8Sm6Cucqt7L/lCDfWhNvxtpDOwOiUqYjk1nOBpyMYyyRy8/24eZTrkKyfije1egXI0I5YzRMtq7p4PZLaw==";
        const string RedirectUri = "myapp://";
        const string GrantType = "authorization_code";
        const string UserinfoEndpoint = "https://pp.netseidbroker.dk/op/connect/userinfo";

        public async Task<bool> LoginWithMitId()
        {
            try
            {
                WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri("https://pp.netseidbroker.dk/op/connect/authorize?client_id=e298b1aa-85ba-4b9c-97c1-977a51e2b215&redirect_uri=myapp://&response_type=code&scope=openid%20mitid%20userinfo_token"),
                    new Uri("myapp://"));

                string authorizationCode = authResult?.Properties["code"];

                if (!string.IsNullOrEmpty(authorizationCode))
                {

                    string userInfo = await ExchangeAuthorizationCodeForUserInfoTokenAsync(authorizationCode);

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadJwtToken(userInfo);

                    var payload = jsonToken.Payload;

                    var uuid = payload.Where(x => x.Key == "mitid.uuid").SingleOrDefault();
                    var age = payload.Where(x => x.Key == "mitid.age").SingleOrDefault();
                    var dateOfBirth = payload.Where(x => x.Key == "mitid.date_of_birth").SingleOrDefault();
                    var name = payload.Where(x => x.Key == "mitid.identity_name").SingleOrDefault();

                    if (uuid.Value.ToString() != "")
                    {
                        var person = await _personService.GetPersonByUUID((string)uuid.Value);
                        if (person?.UUID != null)
                        {
                            _personService.LogIn(person);

                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await Shell.Current.GoToAsync("//pinkodeLoginPage");
                            });
                        }
                        else
                        {
                            var newPerson = new PersonModel()
                            {
                                UUID = uuid.Value.ToString(),
                                Navn = name.Value.ToString(),
                                Alder = int.Parse(age.Value.ToString()),
                                Fodselsdato = dateOfBirth.Value.ToString(),
                            };

                            if (await _personService.CreatePerson(newPerson))
                            {
                                newPerson = await _personService.GetPersonByUUID((string)uuid.Value);
                                _personService.LogIn(newPerson);

                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    await Shell.Current.GoToAsync("//pinkodeLoginPage");
                                });
                            }
                            else
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;                
                }
            }
            catch (TaskCanceledException ext)
            {
                Console.WriteLine($"Authentication failed: {ext.Message}");
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                return false;
            }

        }

        private async Task<string> ExchangeAuthorizationCodeForUserInfoTokenAsync(string authorizationCode)
        {
            using (HttpClient client = new HttpClient())
            {
                var postData = new Dictionary<string, string>
                {
                    { "client_id", ClientId },
                    { "code", authorizationCode },
                    { "redirect_uri", RedirectUri },
                    { "grant_type", GrantType },
                    { "client_secret", ClientSecret }
                };

                HttpResponseMessage response = await client.PostAsync(TokenEndpoint, new FormUrlEncodedContent(postData));

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(jsonResponse);
                    var accessToken = tokenResponse.AccessToken;

                    return tokenResponse.UserInfoToken;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error exchanging authorization code for access token: {error}");
                }
            }
        }
    }

    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("userinfo_token")]
        public string UserInfoToken { get; set; }
    }


    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                      Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
                      DataScheme = CALLBACK_SCHEME)]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
        const string CALLBACK_SCHEME = "myapp";
    }
}

