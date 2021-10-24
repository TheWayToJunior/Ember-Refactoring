using System.ComponentModel.DataAnnotations;

namespace Ember.Shared
{
    public class BindingRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(6)]
        public string Number { get; set; }
    }
}
