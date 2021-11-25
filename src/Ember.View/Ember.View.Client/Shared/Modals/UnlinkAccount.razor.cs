using Blazored.Modal;
using Blazored.Modal.Services;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Modals
{
    public partial class UnlinkAccount
    {
        string _errorMessage;

        [CascadingParameter]
        BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public string Email { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public INotificationService NotificationService { get; set; }

        private async Task UnlinkAsync()
        {
            var httpResponse = await HttpClient.DeleteAsync($"api/account?email={Email}");

            if (httpResponse.IsSuccessStatusCode)
            {
                BlazoredModal.Close(ModalResult.Cancel());
                NotificationService.Send(this, new MessageNotification("Unlink account"));
            }

            _errorMessage = await ErrorMessageHandler.HandleAsync(httpResponse);
        }
    }
}
