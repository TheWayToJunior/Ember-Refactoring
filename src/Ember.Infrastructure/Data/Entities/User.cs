using Ember.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Ember.Infrastructure.Data.Entitys
{
    public class User : IdentityUser, IUser
    {
        public User(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }
    }
}
