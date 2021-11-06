using Ember.Domain;
using Ember.Domain.Contracts;
using MediatR;

namespace Ember.Application.Features.News.Queries.GetPage
{
    public class GetPageNewsQuery : IRequest<IResult<GetPageNewsResponse>>
    {
        public GetPageNewsQuery() 
            : this(page: 1, pageSize: 5)
        {
        }

        public GetPageNewsQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
            Category = CategoryMode.All;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public CategoryMode Category { get; set; }
    }
}
