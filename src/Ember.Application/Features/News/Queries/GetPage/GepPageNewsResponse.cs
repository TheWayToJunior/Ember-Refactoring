using Ember.Shared.Models;
using System;
using System.Collections.Generic;

namespace Ember.Application.Features.News.Queries.GetPage
{
    public class GetPageNewsResponse
    {
        public IEnumerable<NewsDto> Values { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }

        public int TotalPages => Convert.ToInt32(Math.Ceiling((double)Page / PageSize));
    }
}
