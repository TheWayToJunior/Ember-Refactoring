using Blazored.Modal;
using Blazored.Modal.Services;
using Ember.Shared;
using Ember.View.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Modals
{
    public partial class CreateNews
    {
        private string _error = string.Empty;

        private NewsDTO _news;
        private IDictionary<string, object> _formAttributes;

        public CreateNews()
        {
            _news = new() { Time = DateTime.Now };

            _formAttributes = new Dictionary<string, object>
            {
                { "style", "min-width:500px;" }
            };
        }

        [CascadingParameter]
        public BlazoredModalInstance BlazoredModal { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        public bool ShowErrors => !string.IsNullOrEmpty(_error);

        private async Task CreateAsync()
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(_news), Encoding.UTF8, "application/json");

            var httpResponse = await HttpClient.PostAsync("api/news", httpContent);

            _error = await ErrorMessageHandler.HandleAsync(httpResponse);

            if (string.IsNullOrEmpty(_error))
            {
                BlazoredModal.Close(ModalResult.Cancel());
            }
        }
    }
}
