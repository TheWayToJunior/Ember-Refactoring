using System;

namespace Ember.Exceptions
{
    public class NoSpecifiedElementException : Exception
    {
        public NoSpecifiedElementException() 
            : this("The specified element was not found")
        {
        }

        public NoSpecifiedElementException(string message) 
            : base(message)
        {
        }

        public NoSpecifiedElementException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
