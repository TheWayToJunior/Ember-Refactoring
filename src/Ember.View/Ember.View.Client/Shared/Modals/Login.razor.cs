using Ember.Client.Auth;
using Ember.Shared;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Modals
{
    public partial class Login
    {
        private string _error = string.Empty;

        private AuthenticationReques _userInfo;

        private readonly Dictionary<string, object> _editFormAttributes;

        public Login()
        {
            _editFormAttributes = new Dictionary<string, object>
            {
                { "style", "min-width:290px;" }
            };

            _userInfo = new AuthenticationReques();
        }

        public bool ShowErorr => !string.IsNullOrEmpty(_error);

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IAuthenticationProvider LoginService { get; set; }

        async Task LoginUser()
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(_userInfo), Encoding.UTF8, "application/json");

            var httpResponse = await Http.PostAsync("api/authentication/login", httpContent);

            try
            {
                var userToken = await httpResponse.Content.DeserializeResultAsync<UserToken>();

                await LoginService.Login(userToken);
                await NavigateToAsync();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
        }

        private async Task NavigateToAsync()
        {
            var authenticationState = await AuthenticationStateTask;

            if (authenticationState?.User?.Identity?.IsAuthenticated ?? false)
            {
                var returnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    Console.WriteLine(returnUrl);
                    NavigationManager.NavigateTo($"/{returnUrl}"); return;
                }

                NavigationManager.NavigateTo("/account");
            }
        }
    }
}
