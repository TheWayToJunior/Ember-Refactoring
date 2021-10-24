namespace Ember.Shared
{
    public class PaginationRequest
    {
        public PaginationRequest(int page = 1, int pageSize = 5)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
