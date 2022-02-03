using Blazored.Modal;
using Ember.Client.Helpers;
using Ember.Shared;
using Ember.Shared.Responses;
using Ember.Web.Client.Helpers;
using Ember.Web.Client.Shared.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Web.Client.ViewModels
{
    public class AdminModel : BaseModel
    {
        private const int PageSize = 5;

        public AdminModel()
        {
            Links = new Dictionary<string, object>
            {
                { "All",       Roles.User },
                { "Consumers", Roles.Consumer },
                { "Editors",   Roles.Editor },
                { "Admins",    Roles.Admin }
            };

            CurrentPage = 1;
            CurrentCategory = Roles.User;
        }

        public IEnumerable<UserRolesDTO> UserRoles { get; private set; }

        public IDictionary<string, object> Links { get; }

        public int CurrentPage { get; private set; }

        public string CurrentCategory { get; private set; }

        public int TotalPages { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshPageAsync();
        }

        private async Task RefreshPageAsync()
        {
            await GetUserRolesAsync();

            var statistic = await GetRoleStatisticsAsync();
            var data = statistic
                .Where(rs => rs.RoleName != Roles.User)
                .OrderBy(rs => rs.UsersCount);

            await JsRuntime.InitPieChart(data);
            StateHasChanged();
        }

        public async Task SelectedLinkAsync(object category)
        {
            CurrentPage = 1;
            CurrentCategory = category.ToString();

            await GetUserRolesAsync();
        }

        public async Task SelectedPageAsync(int page)
        {
            CurrentPage = page;

            await GetUserRolesAsync();
        }

        private async Task GetUserRolesAsync()
        {
            var httpResponse = await HttpClient
               .GetAsync($"api/UserRoles?page={CurrentPage}&pageSize={PageSize}&role={CurrentCategory}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(httpResponse.StatusCode); return;
            }

            var responsString = await httpResponse.Content.ReadAsStringAsync();
            var response = ResultSerializer.Deserialize<PaginationResponse<UserRolesDTO>>(responsString);

            UserRoles = response.Values;
            TotalPages = response.TotalPages;
        }

        private async Task<IEnumerable<RoleStatistics>> GetRoleStatisticsAsync()
        {
            var httpResponse = await HttpClient.GetAsync("api/UserRoles/GetRoleStatistics");

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(httpResponse.StatusCode);
            }

            var responsString = await httpResponse.Content.ReadAsStringAsync();
            return ResultSerializer.Deserialize<IEnumerable<RoleStatistics>>(responsString);
        }

        public async Task ShowEditRolesAsync(UserRolesDTO user)
        {
            var modalParameters = new ModalParameters();

            modalParameters.Add(nameof(EditRoles.UserRoles), user);

            var modal = Modal.Show<EditRoles>($"Роли {user.Email}", modalParameters);
            var result = await modal.Result;

            if (result.Cancelled)
            {
                await RefreshPageAsync();
            }
        }
    }
}
