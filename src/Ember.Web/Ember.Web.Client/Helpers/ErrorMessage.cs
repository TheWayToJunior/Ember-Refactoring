using Ember.Shared;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.Web.Client.Helpers
{
    public static class ErrorMessageHandler
    {
        public static async Task<string> HandleAsync(HttpResponseMessage responseMessage)
        {
            var responsString = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OperationResult>(responsString,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result.Errors.FirstOrDefault();
        }
    }
}
