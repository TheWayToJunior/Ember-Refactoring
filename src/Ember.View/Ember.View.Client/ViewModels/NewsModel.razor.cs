using Blazored.Modal;
using Ember.Client.Helpers;
using Ember.Domain;
using Ember.Shared.Models;
using Ember.Shared.Responses;
using Ember.View.Client.Helpers;
using Ember.View.Client.Shared.Modals;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ember.View.Client.ViewModels
{
    public class NewsModel : BaseModel
    {
        private const int PageSize = 5;

        public NewsModel()
        {
            Links = new Dictionary<string, object>()
            {
                { "Все",      CategoryMode.All },
                { "События",  CategoryMode.Events },
                { "Ремонт",   CategoryMode.Repair },
                { "Экология", CategoryMode.Ecology }
            };

            CurrentPage = 1;
            CurrentCategory = CategoryMode.All;
        }

        public IEnumerable<NewsDto> News { get; private set; }

        public IDictionary<string, object> Links { get; }

        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public CategoryMode CurrentCategory { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await GetNewsAsync();
        }

        public async Task SelectedLinkAsync(object category)
        {
            CurrentPage = 1;
            CurrentCategory = (CategoryMode)category;

            await GetNewsAsync();
        }

        public async Task SelectedPageAsync(int page)
        {
            CurrentPage = page;

            await GetNewsAsync();
            await JsRuntime.ScrollToElementId("body");
        }

        public async Task GetNewsAsync()
        {
            var httpResponse = await HttpClient
                .GetAsync($"api/News?page={CurrentPage}&pageSize={PageSize}&category={CurrentCategory}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(httpResponse.StatusCode); return;
            }

            var responsString = await httpResponse.Content.ReadAsStringAsync();
            var response = ResultSerializer.Deserialize<PaginationResponse<NewsDto>>(responsString);

            News = response.Values;
            TotalPages = response.TotalPages;
        }

        public async Task CreatePostAsync()
        {
            var modal = Modal.Show<CreateNews>($"Добавление новости {DateTime.Now.ToShortDateString()}");

            var result = await modal.Result;

            if (result.Cancelled)
            {
                await GetNewsAsync();
            }
        }

        public async Task EditPostAsync(NewsDto post)
        {
            ModalParameters parameters = new();
            parameters.Add(nameof(EditNews.NewsPost), post);

            var modal = Modal.Show<EditNews>($"Изменить новости {post.Time.ToShortDateString()}", parameters);

            var result = await modal.Result;

            if (result.Cancelled)
            {
                await GetNewsAsync();
            }
        }

        public async Task DeletePostAsync(int id)
        {
            await HttpClient.DeleteAsync($"api/News/{id}");
            await GetNewsAsync();
        }
    }
}
