using System;
using System.Collections.Generic;

namespace Ember.Shared.Responses
{
    public class PaginationResponse<TValue>
    {
        public IEnumerable<TValue> Values { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }

        public int TotalPages => Convert.ToInt32(Math.Ceiling((double)Page / PageSize));
    }
}
