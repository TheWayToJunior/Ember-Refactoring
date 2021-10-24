using System.Collections.Generic;

namespace Ember.Shared.Models
{
    public class UserRoles
    {
        public string Email { get; set; }

        public IList<string> AllRoles { get; set; }

        public IList<string> AllUserRoles { get; set; }
    }
}
