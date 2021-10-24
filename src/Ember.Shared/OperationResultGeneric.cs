﻿using System.Collections.Generic;

namespace Ember.Shared
{
    public class OperationResult<T> : OperationResult, IResult<T>
       where T : class
    {
        public OperationResult(T value = null)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public OperationResult<T> SetValue(T responseObj)
        {
            Value = responseObj;
            return this;
        }

        public static new OperationResultBuilder<T> CreateBuilder()
        {
            return new OperationResultBuilder<T>();
        }
    }

    public class OperationResultBuilder<T> : OperationResultBuilder
        where T : class
    {
        public OperationResultBuilder()
        {
            OperationResult = new OperationResult<T>();
        }

        public override OperationResultBuilder<T> AppendError(string message)
        {
            return base.AppendError(message) as OperationResultBuilder<T>;
        }

        public override OperationResultBuilder<T> AppendErrors(IEnumerable<string> messages)
        {
            return base.AppendErrors(messages) as OperationResultBuilder<T>;
        }

        public override IResult<T> BuildResult()
        {
             return base.BuildResult() as OperationResult<T>;
        }

        public OperationResultBuilder<T> SetValue(T value)
        {
            (OperationResult as OperationResult<T>).SetValue(value);
            return this;
        }
    }
}
