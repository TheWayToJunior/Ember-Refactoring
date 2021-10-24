using Ember.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Ember.Infrastructure.Data.Entitys
{
    public class ApplicationUser : IdentityUser, IUser
    {
        public ApplicationUser(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }
    }
}
