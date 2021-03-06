using Ember.Domain.Contracts;
using System.Collections.Generic;

namespace Ember.Shared
{
    public class OperationResult : IResult
    {
        private List<string> _errors;

        public OperationResult()
        {
            _errors = new List<string>();
        }

        public IEnumerable<string> Errors { get => _errors; set => AddErrors(value); }

        public bool IsSuccess => _errors.Count == 0;

        public OperationResult AddErrors(IEnumerable<string> exceptions)
        {
            _errors.AddRange(exceptions);
            return this;
        }

        public static OperationResultBuilder CreateBuilder() => new();
    }

    public class OperationResultBuilder
    {
        private ICollection<string> _errors;
        protected OperationResult OperationResult { get; set; }

        public OperationResultBuilder()
        {
            OperationResult = new OperationResult();
            _errors = CreateCollection();
        }

        public virtual IResult BuildResult()
        {
            OperationResult.AddErrors(_errors);

            return OperationResult;
        }

        public virtual OperationResultBuilder AppendError(string message)
        {
            _errors.Add(message);
            return this;
        }

        public virtual OperationResultBuilder AppendErrors(IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                _errors.Add(message);
            }

            return this;
        }

        protected virtual ICollection<string> CreateCollection()
        {
            return new List<string>();
        }
    }
}
