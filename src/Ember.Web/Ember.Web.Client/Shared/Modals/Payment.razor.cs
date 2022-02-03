using Blazored.Modal;
using Blazored.Modal.Services;
using Ember.Application.Features.Payment.Command.Pay;
using Ember.Shared;
using Ember.Shared.Models;
using Ember.Web.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ember.Web.Client.Shared.Modals
{
    public partial class Payment
    {
        private СreditСard creditСard;
        private readonly IDictionary<string, object> formAttributes;

        public Payment()
        {
            creditСard = new();

            formAttributes = new Dictionary<string, object>
            {
                { "id", "paymentForm" },
                { "action", "#" }
            };
        }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public INotificationService NotificationService { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [CascadingParameter]
        public BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public AccountDTO Account { get; set; }

        public EditContext EditContext { get; set; }

        public string NumberFormat => CardFormat(creditСard.Number);

        public string NameFormat => creditСard.Name;

        public string YearFormat => creditСard.Year;

        public decimal Amount { get; set; } = 750.00m;

        private static string CardFormat(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return Regex.Replace(value, @"(\w{4})(\w{4})(\w{4})(\w{4})", @"$1 $2 $3 $4");
        }

        protected override void OnInitialized()
        {
            EditContext = new(creditСard);
            EditContext.SetFieldCssClassProvider(new CardFieldClassProvider());
        }

        public async Task PayAsync()
        {
            var command = new CreatePayCommand() { NumberAccount = Account.Number, Amount = Amount, СreditСard = creditСard };
            var httpContent = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            var result = await HttpClient.PostAsync("api/Payment", httpContent);

            var error = await ErrorMessageHandler.HandleAsync(result);

            if (string.IsNullOrEmpty(error))
            {
                NotificationService.Send(this, new MessageNotification("Payment completed successfully"));

                await Task.Delay(300);
                BlazoredModal.Close(ModalResult.Cancel());
            }
        }
    }

    public class CardFieldClassProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext,
            in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return isValid ? string.Empty : "invalide-input";
        }
    }
}
