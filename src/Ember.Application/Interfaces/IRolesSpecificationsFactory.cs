using Ember.Application.Specification;
using System.Collections.Generic;

namespace Ember.Application.Interfaces
{
    public interface IRolesSpecificationsFactory
    {
        SpecificationsCollection Create(IEnumerable<string> userRoles, IEnumerable<string> changRoles);
    }
}
