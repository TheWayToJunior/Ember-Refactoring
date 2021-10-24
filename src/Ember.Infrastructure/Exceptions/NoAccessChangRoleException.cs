using System;

namespace Ember.Server.Exceptions
{
    public class NoAccessChangRoleException : Exception
    {
        public NoAccessChangRoleException()
        {
        }

        public NoAccessChangRoleException(string message)
            : base(message)
        {
        }

        public NoAccessChangRoleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
