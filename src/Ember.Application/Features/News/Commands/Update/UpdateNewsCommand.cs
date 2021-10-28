using Ember.Domain;
using Ember.Domain.Contracts;
using MediatR;
using System;

namespace Ember.Application.Features.News.Commands.Update
{
    public class UpdateNewsCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageSrc { get; set; }

        public string Source { get; set; }

        public DateTime Time { get; set; }

        public CategoryMode Category { get; set; }
    }
}
