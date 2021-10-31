using System;
using System.Collections.Generic;

namespace Ember.Shared.Responses
{
    public class PaginationResponse<TValue>
    {
        public PaginationResponse()
        {
            Values = new List<TValue>();
        }

        public PaginationResponse(IEnumerable<TValue> values, int page, int pageSize, int totalSize)
        {
            Values = values;
            Page = page;
            PageSize = pageSize;
            TotalSize = totalSize;
        }

        public IEnumerable<TValue> Values { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalSize { get; init; }

        public int TotalPages => Convert.ToInt32(Math.Ceiling((double)TotalSize / PageSize));
    }
}
