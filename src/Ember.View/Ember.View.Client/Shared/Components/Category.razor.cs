using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.View.Client.Shared.Components
{
    public partial class Category
    {
        [Parameter]
        public object CurrentCategory { get; set; }

        [Parameter]
        public IDictionary<string, object> Links { get; set; }

        [Parameter]
        public EventCallback<object> SelectedLink { get; set; }

        private List<LinkModel> links;

        protected override void OnParametersSet()
        {
            if(CurrentCategory is null)
            {
                throw new ArgumentNullException(nameof(CurrentCategory));
            }

            LoadPages();
        }

        private async Task SelectedLinkInternal(LinkModel link)
        {
            if (link.Active)
            {
                return;
            }

            await SelectedLink.InvokeAsync(link.Category);
        }

        private void LoadPages()
        {
            links = new List<LinkModel>();

            foreach (var key in Links.Keys)
            {
                links.Add(new LinkModel(key, Links[key])
                {
                    Active = CurrentCategory.Equals(Links[key])
                });
            }
        }

        class LinkModel
        {
            public LinkModel(string text, object сategory)
            {
                Category = сategory;
                Text = text;
            }

            public string Text { get; set; }

            public object Category { get; set; }

            public bool Active { get; set; } = false;
        }
    }
}
