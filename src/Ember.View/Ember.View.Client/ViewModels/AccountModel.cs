using Blazored.Modal;
using Ember.Client.Helpers;
using Ember.Shared;
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
        private AuthenticationState _authenticationState;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IAccountManager AccountManager { get; set; }

        public AccountDTO Account { get; private set; }

        public string Email { get; private set; }


        protected override async Task OnInitializedAsync()
        {
            _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            Email = _authenticationState.User.Identity.Name;
            await GetAccountAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Account != null && !firstRender)
            {
                await InitChartBarAsync();
                await InitLineCharAsync();
            }
        }

        public async Task GetAccountAsync()
        {
            Account = await AccountManager.GetAccountAsync();
        }

        public async Task ShowPaymentModalAsync()
        {
            if (!AccountManager.IsRelatedAccount) return;

            var parameter = (nameof(Account), Account);
            await ShowAsync<Payment>("Оплата счета", parameter);
        }

        public async Task ShowModalAsync()
        {
            /// Limiting the ability to link an account for an admin
            if (!_authenticationState.User.IsInRole(Roles.Consumer)) return;

            var parameter = (nameof(BindAccount.Email), Email);

            if (AccountManager.IsRelatedAccount)
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

        private async Task InitChartBarAsync()
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

        private async Task InitLineCharAsync()
        {
            if (!AccountManager.IsRelatedAccount)
            {
                await JsRuntime.InitLineChar(new List<int>());
                return;
            }

            var data = new decimal[12];
            var accruals = Account.Accruals.Where(a => a.Date.Year == DateTime.Now.Year);

            foreach (var accrual in accruals)
            {
                /// The schedule starts from October
                data[Math.Abs(accrual.Date.Month - 10)] = accrual.Amount;
            }

            await JsRuntime.InitLineChar(data);
        }
    }
}
