using Ember.Shared;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Client.Helpers
{
    public static class JsHelper
    {
        public static ValueTask<bool> ScrollToElementId(this IJSRuntime JSRuntime, string elementId)
        {
            return JSRuntime.InvokeAsync<bool>("scrollToElementId", elementId);
        }

        public static async ValueTask InitSlick(this IJSRuntime JSRuntime)
        {
            await JSRuntime.InvokeVoidAsync("initSlick");
        }

        public static async ValueTask InitCounter(this IJSRuntime JSRuntime)
        {
            await JSRuntime.InvokeVoidAsync("initCounter");
        }

        public static async ValueTask InitMap(this IJSRuntime JSRuntime)
        {
            await JSRuntime.InvokeVoidAsync("initMap");
        }

        public static async ValueTask SetInLocalStorage(this IJSRuntime JSRuntime, string key, string value)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public static async ValueTask<string> GetFromLocalStorage(this IJSRuntime JSRuntime, string key)
        {
            return await JSRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        public static async ValueTask RemoveItem(this IJSRuntime JSRuntime, string key)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public static async ValueTask InitPieChart(this IJSRuntime JSRuntime, IEnumerable<RoleStatistics> statistics)
        {
            await JSRuntime.InvokeVoidAsync("initPieChart", 
                statistics.Select(rs => rs.RoleName),
                statistics.Select(rs => rs.UsersCount));
        }

        public static async ValueTask InitChartBar(this IJSRuntime JSRuntime, object weekday)
        {
            await JSRuntime.InvokeVoidAsync("initChartBar", weekday);
        }
    }
}
