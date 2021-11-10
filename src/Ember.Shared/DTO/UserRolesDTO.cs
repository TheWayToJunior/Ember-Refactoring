using System.Collections.Generic;

namespace Ember.Shared
{
    public class UserRolesDTO
    {
        public UserRolesDTO()
        {
            Roles = new List<string>();
            UserRoles = new List<string>();
        }

        public UserRolesDTO(string email, IEnumerable<string> roles, IEnumerable<string> userRoles)
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
