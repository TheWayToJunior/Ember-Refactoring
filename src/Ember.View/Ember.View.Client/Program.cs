using Blazored.Modal;
using Ember.Client.Auth;
using Ember.View.Client;
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

            builder.Services.AddScoped<AuthenticationProvider>()
                .AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthenticationProvider>())
                .AddScoped<IAuthenticationProvider>(provider => provider.GetRequiredService<AuthenticationProvider>());

            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
