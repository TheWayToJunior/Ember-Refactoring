using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server.Exceptions
{
    public class NoSpecifiedElementException : Exception
    {
        public NoSpecifiedElementException() : this("The specified element was not found")
        {
        }

        public NoSpecifiedElementException(string message) : base(message)
        {
        }

        public NoSpecifiedElementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
