using Ember.Domain;
using System;

namespace Ember.Application.Dto
{
    public class GetNewsResponse
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
