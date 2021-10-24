using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Ember.Shared
{
    public class UserRoles
    {
        public string Email { get; set; }

        public IList<IdentityRole> AllRoles { get; set; }

        public IList<string> AllUserRoles { get; set; }
    }
}
