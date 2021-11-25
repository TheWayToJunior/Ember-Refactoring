using Ember.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.View.Client
{
    public interface IAccountManager
    {
        AccountDTO Account { get; }

        bool IsRelatedAccount { get; }

        Task<AccountDTO> GetAccountAsync();

        Task<IEnumerable<PaymentDTO>> GetPaymentHistory();
    }

    public class AccountManager : IAccountManager
    {
        private readonly AuthenticationStateProvider _authenticationProvider;
        private readonly IAccountService _accountService;


        public AccountManager(AuthenticationStateProvider authenticationProvider, IAccountService accountService)
        {
            _authenticationProvider = authenticationProvider;
            _accountService = accountService;
        }

        public AccountDTO Account { get; set; }

        public bool IsRelatedAccount => !string.IsNullOrEmpty(Account?.Number);

        public async Task<AccountDTO> GetAccountAsync()
        {
            await SetAccountAsync();
            return Account;
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentHistory()
        {
            if (Account is null)
            {
                await SetAccountAsync();
            }

            return Account.Payments;
        }

        private async Task SetAccountAsync()
        {
            var state = await _authenticationProvider.GetAuthenticationStateAsync();
            Account = await _accountService.GetAccountAsync(state.User.Identity.Name);
        }
    }
}
