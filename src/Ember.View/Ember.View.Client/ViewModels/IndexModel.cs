using Ember.Client.Helpers;
using Ember.Shared;
using Ember.Shared.Responses;
using Ember.View.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.View.Client.ViewModels
{
    public class IndexModel : BaseModel
    {
        private const int PageSize = 3;

        public IndexModel()
        {
            Contacts = new[]
            {
                ("home", "г. Горловка ул. Ушева 21"),
                ("email", "gpdonbass@gmail.com"),
                ("phonelink_ring", "+380(95) 091-72-01")
            };

            Slides = new[]
            {
                "https://de.com.ua/uploads/thumb/0/home_energyslide/123-E54J1357.jpg",
                "https://sun9-2.userapi.com/c850720/v850720430/ed5a9/wYDq1V8TND4.jpg",
                "https://sun9-19.userapi.com/c850632/v850632256/12cc53/KZIhDPJ7bp0.jpg",
                "https://de.com.ua/uploads/thumb/0/home_energyslide/116-E54J3014.jpg",
                "https://de.com.ua/uploads/thumb/0/home_energyslide/117-E54J2966.jpg",
                "https://sun9-28.userapi.com/c854224/v854224084/12aa7f/aJlUY851MCw.jpg"
            };
        }

        public IEnumerable<(string, string)> Contacts { get; }

        public string[] Slides { get; }

        public IEnumerable<NewsDTO> News { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            var httpResponse = await HttpClient
                .GetAsync($"api/News?pageSize={PageSize}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(httpResponse.StatusCode); return;
            }

            var responsString = await httpResponse.Content.ReadAsStringAsync();
            var response = ResultSerializer.Deserialize<PaginationResponse<NewsDTO>>(responsString);

            News = response.Values;

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InitSlick();
            }

            await JsRuntime.InitMap();
        }
    }
}
