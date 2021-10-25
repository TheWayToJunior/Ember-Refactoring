using Ember.Domain.Contracts;
using Ember.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Ember.Infrastructure.Data.Entitys
{
    public class ApplicationUser : IdentityUser, IUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
