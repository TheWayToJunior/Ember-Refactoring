using Ember.Shared;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ember.Web.Client.Helpers
{
    public static class ResultSerializer
    {
        public static T Deserialize<T>(string responsString)
            where T : class
        {
            var result = JsonSerializer.Deserialize<OperationResult<T>>(responsString,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            if (!result.IsSuccess)
            {
                throw new Exception(string.Join("\n", result.Errors));
            }

            return result.Value;
        }

        public static async Task<T> DeserializeResultAsync<T>(this HttpContent content)
            where T : class
        {
            var responsString = await content.ReadAsStringAsync();
            return Deserialize<T>(responsString);
        }
    }
}
