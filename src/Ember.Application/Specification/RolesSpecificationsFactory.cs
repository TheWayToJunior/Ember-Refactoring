using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ember.Application.Specification
{
    public class RolesSpecificationsFactory
    {
        protected virtual ISpecification CreateAdminRoleSpecification(IEnumerable<string> usetRoles, IEnumerable<string> changRoles)
        {
            var condition = new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin));

            return new OrSpecification(
                new NoChangeRoleSpecification(usetRoles, condition),
                new NoChangeRoleSpecification(changRoles, condition));
        }

        protected virtual ISpecification CreateUserRoleSpecification(IEnumerable<string> roles)
        {
            return new NoChangeRoleSpecification(roles,
                new DelegateCondition<IEnumerable<string>>(@roles => !@roles.Contains(Roles.User)));
        }

        public SpecificationsCollection Create(IEnumerable<string> usetRoles, IEnumerable<string> changRoles)
        {
            return new SpecificationsCollection(
                CreateAdminRoleSpecification(usetRoles, changRoles),
                CreateUserRoleSpecification(changRoles));
        }
    }
}
