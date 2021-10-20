using Blazored.Modal;
using Ember.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ember.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<JWTAuthenticationProvider>()
                .AddScoped<AuthenticationStateProvider, JWTAuthenticationProvider>(provider => 
                    provider.GetRequiredService<JWTAuthenticationProvider>())
                .AddScoped<ILoginService, JWTAuthenticationProvider>(provider => 
                    provider.GetRequiredService<JWTAuthenticationProvider>());

            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
