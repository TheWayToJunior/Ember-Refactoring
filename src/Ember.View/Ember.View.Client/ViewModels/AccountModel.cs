using Blazored.Modal;
using Ember.Client.Helpers;
using Ember.Shared;
using Ember.View.Client.Helpers;
using Ember.View.Client.Shared.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.View.Client.ViewModels
{
    public class AccountModel : BaseModel
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public AccountDto Account { get; private set; }

        public string Email { get; private set; }

        private bool IsRelated => !string.IsNullOrEmpty(Account.Number);

        private static AccountDto EmptyAccount => new();

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            Email = state.User.Identity.Name;
            await GetAccountAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Account != null && !firstRender)
            {
                await InitChartBar();
            }
        }

        public async Task GetAccountAsync()
        {
            var httpResponse = await HttpClient.GetAsync($"api/account?email={Email}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(httpResponse.StatusCode);
            }

            try
            {
                Account = await httpResponse.Content.DeserializeResultAsync<AccountDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Account = EmptyAccount;
            }
        }

        public async Task ShowModalAsync()
        {
            if (IsRelated)
            {
                Console.WriteLine(IsRelated);
                Console.WriteLine(Account.Address);
                await ShowAsync<UnlinkAccount>("Отвязать аккаунт"); return;
            }

            await ShowAsync<BindAccount>("Привязать аккаунт");
        }

        private async Task ShowAsync<T>(string title)
            where T : ComponentBase
        {
            ModalParameters parameters = new();
            parameters.Add(nameof(BindAccount.Email), Email);

            var modal = Modal.Show<T>(title, parameters);

            var result = await modal.Result;

            if (result.Cancelled)
            {
                await GetAccountAsync();
            }
        }

        private async Task InitChartBar()
        {
            object weekday = new();

            var deys = Account.Payments?.Select(h => h.Date);

            if (deys != null)
            {
                weekday = new
                {
                    Monday = GetQuantityDays(deys, DayOfWeek.Monday),
                    Tuesday = GetQuantityDays(deys, DayOfWeek.Tuesday),
                    Wednesday = GetQuantityDays(deys, DayOfWeek.Wednesday),
                    Thursday = GetQuantityDays(deys, DayOfWeek.Thursday),
                    Friday = GetQuantityDays(deys, DayOfWeek.Friday),
                };
            }

            await JsRuntime.InitChartBar(weekday);
        }

        private int GetQuantityDays(IEnumerable<DateTime> deys, DayOfWeek day)
        {
            return deys.Where(d => d.DayOfWeek == day).Count();
        }
    }
}
