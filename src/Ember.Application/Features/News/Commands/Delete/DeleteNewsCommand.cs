using Ember.Shared;
using MediatR;

namespace Ember.Application.Features.News.Commands.Delete
{
    public class DeleteNewsCommand : IRequest<IResult>
    {
        public int Id { get; set; }
    }
}
