using Blazored.Modal;
using Blazored.Modal.Services;
using Ember.Shared;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Modals
{
    public partial class BindAccount
    {
        private string _errorMessage;

        private BindingRequest _bindingRequest;

        [CascadingParameter]
        public BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public string Email { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        protected override void OnInitialized()
        {
            _bindingRequest = new BindingRequest
            {
                Email = this.Email
            };
        }

        private async Task Bind()
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(_bindingRequest), Encoding.UTF8, "application/json");

            var httpResponse = await HttpClient.PostAsync("api/account", httpContent);

            _errorMessage = await ErrorMessageHandler.HandleAsync(httpResponse);

            if(string.IsNullOrEmpty(_errorMessage))
            {
                BlazoredModal.Close(ModalResult.Cancel());
            }
        }
    }
}
