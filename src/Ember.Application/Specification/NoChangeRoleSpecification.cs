using Ember.Exceptions;
using System.Collections.Generic;

namespace Ember.Application.Specification
{
    public class NoChangeRoleSpecification : ISpecification
    {
        private readonly IEnumerable<string> _roles;
        private readonly DelegateCondition<IEnumerable<string>> _condition;

        public NoChangeRoleSpecification(IEnumerable<string> roles, DelegateCondition<IEnumerable<string>> condition)
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
