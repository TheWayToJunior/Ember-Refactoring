using Blazored.Modal;
using Blazored.Modal.Services;
using Ember.Shared;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Modals
{
    public partial class EditRoles
    {
        private string _error = string.Empty;

        private List<RoleModifier> _modifiers;

        [Inject]
        public HttpClient HttpClient { get; set; }

        [CascadingParameter]
        public BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public UserRolesDTO UserRoles { get; set; }

        private bool ShowErrors => !string.IsNullOrEmpty(_error);


        protected override void OnParametersSet()
        {
            _modifiers = new List<RoleModifier>();

            foreach (var role in UserRoles.Roles)
            {
                _modifiers.Add(new RoleModifier(role, UserRoles.UserRoles.Any(r => r == role)));
            }
        }

        private async Task Change()
        {
            var list = _modifiers.Where(m => m.IsChosen).Select(m => m.Role);

            var httpContent = new StringContent(JsonSerializer.Serialize(list), Encoding.UTF8, "application/json");

            var httpResponse = await HttpClient.PutAsync($"api/userroles/edit?email={UserRoles.Email}", httpContent);

            _error = await ErrorMessageHandler.HandleAsync(httpResponse);

            if (string.IsNullOrEmpty(_error))
            {
                BlazoredModal.Close(ModalResult.Cancel());
            }
        }

        class RoleModifier
        {
            public string Role { get; private set; }

            public bool IsChosen { get; set; }

            public RoleModifier(string role, bool isChosen)
            {
                this.Role = role;
                this.IsChosen = isChosen;
            }
        }
    }
}
