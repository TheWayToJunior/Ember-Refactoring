using Ember.Shared;
using Ember.View.Client.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ember.View.Client
{

    public interface IAccountService
    {
        Task<AccountDTO> GetAccountAsync(string email);
    }

    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AccountDTO> GetAccountAsync(string email)
        {
            var httpResponse = await _httpClient.GetAsync($"api/account?email={email}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                /// TODO: Add Logging
                Console.WriteLine(httpResponse.StatusCode);
            }

            AccountDTO account = new();

            try
            {
                account = await httpResponse.Content.DeserializeResultAsync<AccountDTO>();
            }
            catch (Exception ex)
            {
                /// TODO: Add Logging
                Console.WriteLine(ex.Message);
            }

            return account;
        }
    }
}
