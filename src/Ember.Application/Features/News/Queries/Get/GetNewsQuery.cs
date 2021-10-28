using Ember.Application.Dto;
using Ember.Domain.Contracts;
using MediatR;

namespace Ember.Application.Features.News.Queries
{
    public class GetNewsQuery : IRequest<IResult<GetNewsResponse>>
    {
        public int Id { get; set; }
    }
}
