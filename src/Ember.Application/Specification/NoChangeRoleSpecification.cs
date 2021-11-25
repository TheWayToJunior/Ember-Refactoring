using Ember.Domain.Contracts;
using Ember.Exceptions;
using System.Collections.Generic;

namespace Ember.Application.Specification
{
    public class NoChangeRoleSpecification : ISpecification
    {
        private readonly IEnumerable<string> _roles;
        private readonly ICondition<IEnumerable<string>> _condition;

        public NoChangeRoleSpecification(IEnumerable<string> roles, ICondition<IEnumerable<string>> condition)
        {
            _roles = roles;
            _condition = condition;
        }

        public void CheckExecution()
        {
            if (_condition.IsMatch(_roles))
            {
                throw new NoAccessChangRoleException($"Cannot change the role");
            }
        }
    }
}
