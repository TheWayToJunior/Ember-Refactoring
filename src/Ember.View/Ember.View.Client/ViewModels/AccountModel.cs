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

        public AccountDTO Account { get; private set; }

        public string Email { get; private set; }

        private bool IsRelated => !string.IsNullOrEmpty(Account.Number);

        private static AccountDTO EmptyAccount => new();

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
                Account = await httpResponse.Content.DeserializeResultAsync<AccountDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Account = EmptyAccount;
            }
        }

        public async Task ShowPaymentModalAsync()
        {
            if (IsRelated)
            {
                var parameter = (nameof(Account), Account);
                await ShowAsync<Payment>("Оплата счета", parameter);
            }
        }

        public async Task ShowModalAsync()
        {
            var parameter = (nameof(BindAccount.Email), Email);

            if (IsRelated)
            {
                await ShowAsync<UnlinkAccount>("Отвязать аккаунт", parameter); return;
            }

            await ShowAsync<BindAccount>("Привязать аккаунт", parameter);
        }

        private async Task ShowAsync<T>(string title, params (string, object)[] parameters)
            where T : ComponentBase
        {
            ModalParameters modalParameters = new();
            foreach ((string name, object value) in parameters)
            {
                modalParameters.Add(name, value);
            }


            var modal = Modal.Show<T>(title, modalParameters);

            var result = await modal.Result;

            if (result.Cancelled)
            {
                await GetAccountAsync();
            }
        }

        private async Task InitChartBar()
        {
            List<int> counts = new();

            var deys = Account.Payments?.Select(h => h.Date);

            if (deys != null)
            {
                foreach (var day in Enum.GetValues<DayOfWeek>())
                {
                    counts.Add(deys.Where(d => d.DayOfWeek == day).Count());
                }
            }

            await JsRuntime.InitChartBar(counts);
        }
    }
}
