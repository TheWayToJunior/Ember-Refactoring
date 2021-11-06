using Ember.Domain;
using Ember.Domain.Contracts;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ember.Application.Features.News.Commands.Create
{
    public class CreateNewsCommand : IRequest<IResult>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string ImageSrc { get; set; }

        public string Source { get; set; }

        public DateTime Time { get; set; }

        public CategoryMode Category { get; set; }
    }
}
