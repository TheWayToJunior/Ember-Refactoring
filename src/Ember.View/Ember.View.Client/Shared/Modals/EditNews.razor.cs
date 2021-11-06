using Blazored.Modal;
using Blazored.Modal.Services;
using Ember.Shared.Models;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Modals
{
    public partial class EditNews
    {
        private string _error;

        private readonly IDictionary<string, object> _editFormAttributes;

        public EditNews()
        {
            _error = string.Empty;

            _editFormAttributes = new Dictionary<string, object>
            {
                { "style", "min-width:500px;" }
            };
        }

        [CascadingParameter]
        public BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public NewsDto NewsPost { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        private async Task EditPostAsync()
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(NewsPost), Encoding.UTF8, "application/json");
            var httpResponse = await HttpClient.PutAsync($"api/news", httpContent);

            _error = await ErrorMessageHandler.HandleAsync(httpResponse);

            if (string.IsNullOrEmpty(_error))
            {
                BlazoredModal.Close(ModalResult.Cancel());
            }
        }
    }
}
