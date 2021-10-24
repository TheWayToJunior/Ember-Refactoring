using System;

namespace Ember.Shared
{
    public class UserTokenResponse
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
