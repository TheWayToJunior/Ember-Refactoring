using System.Collections.Generic;

namespace Ember.Shared
{
    public class UserRolesDto
    {
        public UserRolesDto()
        {
            Roles = new List<string>();
            UserRoles = new List<string>();
        }

        public UserRolesDto(string email, IEnumerable<string> roles, IEnumerable<string> userRoles)
        {
            Email = email;
            Roles = roles;
            UserRoles = userRoles;
        }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }
}
