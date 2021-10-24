using System.ComponentModel.DataAnnotations;

namespace Ember.Shared
{
    public class MessageRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string TextBody { get; set; }
    }
}
