using System.ComponentModel.DataAnnotations;

namespace Ember.Shared
{
    public class BindingRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(6)]
        public string Number { get; set; }
    }
}
