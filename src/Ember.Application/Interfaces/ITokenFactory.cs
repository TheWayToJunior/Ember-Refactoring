using Ember.Shared;
using System;
using System.Collections.Generic;

namespace Ember.Infrastructure.Services
{
    public interface ITokenFactory
    {
        UserToken CreateToken(IEnumerable<string> userRoles, string userName, string securityKey, DateTime expiration);
    }
}