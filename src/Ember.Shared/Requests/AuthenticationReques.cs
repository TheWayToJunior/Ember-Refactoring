using System.ComponentModel.DataAnnotations;

namespace Ember.Shared
{
    public class AuthenticationReques
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
