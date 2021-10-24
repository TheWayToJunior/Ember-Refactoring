using System.Collections.Generic;

namespace Ember.Shared
{
    public interface IResult
    {
        public IEnumerable<string> Errors { get; }

        public bool IsSuccess { get; }
    }

    public interface IResult<T> : IResult
    {
        T Value { get; }
    }
}
