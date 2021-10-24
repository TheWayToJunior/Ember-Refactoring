using System;
using Ember.Domain.Contracts;

namespace Ember.Domain
{
    public class News : IEntity<int>
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
