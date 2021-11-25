using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ember.Application.Specification
{
    public class RolesSpecificationsFactory
    {
        protected virtual ISpecification CreateAdminRoleSpecification(IEnumerable<string> userRoles, IEnumerable<string> changRoles)
        {
            var condition = new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin));

            return new OrSpecification(
                new NoChangeRoleSpecification(userRoles, condition),
                new NoChangeRoleSpecification(changRoles, condition));
        }

        protected virtual ISpecification CreateUserRoleSpecification(IEnumerable<string> userRoles, IEnumerable<string> changRoles)
        {
            return new NoChangeRoleSpecification(changRoles,
                new DelegateCondition<IEnumerable<string>>(@roles => !@roles.Contains(Roles.User)));
        }

        public SpecificationsCollection Create(IEnumerable<string> userRoles, IEnumerable<string> changRoles)
        {
            return new SpecificationsCollection(
                CreateAdminRoleSpecification(userRoles, changRoles),
                CreateUserRoleSpecification(null, changRoles));
        }
    }
}
