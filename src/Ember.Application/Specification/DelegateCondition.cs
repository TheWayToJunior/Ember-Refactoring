using System;
using Ember.Domain.Contracts;

namespace Ember.Application.Specification
{
	public class DelegateCondition<T> : ICondition<T>
	{
		private Func<T, bool> _condition;

		public DelegateCondition(Func<T, bool> condition)
		{
			_condition = condition;
		}

		public bool IsMatch(T value)
		{
			return _condition(value);
		}
	}
}
