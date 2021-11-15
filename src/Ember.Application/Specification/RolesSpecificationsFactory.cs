using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ember.Application.Specification
{
    public class RolesSpecificationsFactory
    {
        protected virtual ISpecification CreateNoChangeAdminRole(IEnumerable<string> usetRoles, IEnumerable<string> changRoles)
        {
            return new OrSpecification(
                new RoleChangeSpecification(usetRoles,
                    new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin))),
                new RoleChangeSpecification(changRoles,
                    new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin)))
                );
        }

        protected virtual ISpecification CreateNoDeleteUserRole(IEnumerable<string> roles)
        {
            return new RoleChangeSpecification(roles,
                new DelegateCondition<IEnumerable<string>>(@roles => !@roles.Contains(Roles.User)));
        }

        public SpecificationsCollection Create(IEnumerable<string> usetRoles, IEnumerable<string> changRoles)
        {
            return new SpecificationsCollection(
                CreateNoChangeAdminRole(usetRoles, changRoles), 
                CreateNoDeleteUserRole(changRoles));
        }
    }
}
