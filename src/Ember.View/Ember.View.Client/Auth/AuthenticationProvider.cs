using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ember.Client.Helpers;
using Ember.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Ember.Client.Auth
{
    public class AuthenticationProvider : AuthenticationStateProvider, IAuthenticationProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        private const string TokenKey = "TOKENKEY";
        private const string TokenLive = "TOKENLIVE";

        private static AuthenticationState Anonymous => new(new ClaimsPrincipal(new ClaimsIdentity()));

        public AuthenticationProvider(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime  = jsRuntime  ?? throw new ArgumentNullException(nameof(jsRuntime));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _jsRuntime.GetFromLocalStorage(TokenKey);
            string expiration = await _jsRuntime.GetFromLocalStorage(TokenLive);

            if (string.IsNullOrEmpty(token) && string.IsNullOrEmpty(expiration))
            {
                return Anonymous;
            }

            if (DateTime.Now > DateTime.Parse(expiration))
            {
                await Logout();

                return Anonymous;
            }

            return BuildAuthenticationState(token);
        }

        public async Task Login(UserToken userToken)
        {
            await _jsRuntime.SetInLocalStorage(TokenKey, userToken.Token);
            await _jsRuntime.SetInLocalStorage(TokenLive, userToken.Expiration.ToString());

            AuthenticationState authState = BuildAuthenticationState(userToken.Token);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            
            await _jsRuntime.RemoveItem(TokenKey);
            await _jsRuntime.RemoveItem(TokenLive);

            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
