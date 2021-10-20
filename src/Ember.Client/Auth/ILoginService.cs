using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Client.Auth
{
    public interface ILoginService
    {
        Task Login(UserToken userToken);

        Task Logout();
    }
}
