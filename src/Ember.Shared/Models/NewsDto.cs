using Ember.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ember.Shared.Models
{
    public class NewsDto
    {
        [Required]
        public int Id { get; set; }

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
