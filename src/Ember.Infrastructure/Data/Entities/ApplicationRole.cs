using Ember.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Ember.Infrastructure.Data.Entitys
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) 
            : base(roleName)
        {
        }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
