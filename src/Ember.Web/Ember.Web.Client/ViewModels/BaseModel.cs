using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;

namespace Ember.Web.Client.ViewModels
{
    public class BaseModel : ComponentBase
    {

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        [Inject]
        protected IModalService Modal { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected INotificationService NotificationService  { get; set; }
    }
}
