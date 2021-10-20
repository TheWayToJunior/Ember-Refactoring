using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server
{
    public interface IBindAccountService
    {
        Task<Account> GetAccount(string email);

        Task Bind(IdentityUser user, string numberAccount);

        Task Unlink(IdentityUser user);

        Task<bool> CheckBinding(IdentityUser user);
    }
}
