using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ember.Shared
{
    public enum CategoryMode { All, Events, Repair, Ecology };

    public class NewsPost
    {
        public int Id { get; set; }

        [Required]
        public string  Title { get; set; }

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
