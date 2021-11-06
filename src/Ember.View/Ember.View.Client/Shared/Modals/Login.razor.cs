using Ember.Client.Auth;
using Ember.Shared;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
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
        public bool ShowErorr => !string.IsNullOrEmpty(_error);

        private readonly Dictionary<string, object> _editFormAttributes;
        private AuthenticationReques _userInfo;

        public Login()
        {
            _editFormAttributes = new Dictionary<string, object>
            {
                { "style", "min-width:290px;" }
            };

            _userInfo = new AuthenticationReques();
        }

        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILoginService LoginService { get; set; }

        async Task LoginUser()
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(_userInfo), Encoding.UTF8, "application/json");

            var httpResponse = await Http.PostAsync("api/authentication/login", httpContent);

            try
            {
                var userToken = await httpResponse.Content.DeserializeResultAsync<UserToken>();

                await LoginService.Login(userToken);
                NavigationManager.NavigateTo("/account");
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
        }
    }
}
