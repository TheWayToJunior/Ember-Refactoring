using System.ComponentModel.DataAnnotations;

namespace Ember.Shared.Models
{
    public class СreditСard
    {
        [StringLength(maximumLength: 16, MinimumLength = 16), Required]
        public string Number { get; set; }

        [MaxLength(32), Required]
        public string Name { get; set; }

        [Required]
        public string Year { get; set; }

        [StringLength(5), Required(ErrorMessage = "The field is required")]
        public string CVV { get; set; }
    }
}
