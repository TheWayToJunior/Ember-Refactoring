using Ember.Application.Specification;
using Ember.Exceptions;
using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ember.Tests
{
    public class SpecificationsCollectionTests
    {
        [Fact]
        public void RoleChangeSpecification_Admin_NoAccessChangRoleException()
        {
            var roles = new string[] { Roles.Admin, Roles.User, Roles.Consumer };

            var specifications = new SpecificationsCollection(
                new NoChangeRoleSpecification(roles, new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin)))
                );


            var ex = Assert.Throws<NoAccessChangRoleException>(() =>
            {
                specifications.Inspect();
            });


            Assert.NotNull(ex);
            Assert.Equal("Cannot change the role", ex.Message);
        }

        [Fact]
        public void OrSpecification_Admin_NoAccessChangRoleException()
        {
            var changedRoles = new string[] { Roles.Admin, Roles.Consumer };
            var usetRoles = new string[] {  Roles.User, Roles.Consumer };

            var specifications = new SpecificationsCollection(
                new OrSpecification(
                    new NoChangeRoleSpecification(usetRoles,
                        new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin))),
                    new NoChangeRoleSpecification(changedRoles,
                        new DelegateCondition<IEnumerable<string>>(roles => roles.Contains(Roles.Admin)))
                    )
                );


            var ex = Assert.Throws<NoAccessChangRoleException>(() =>
            {
                specifications.Inspect();
            });


            Assert.NotNull(ex);
            Assert.Equal("Cannot change the role", ex.Message);
        }

        [Fact]
        public void RoleChangeSpecification_User_NoAccessChangRoleException()
        {
            var changedRoles = new string[] { Roles.Consumer };

            var specifications = new SpecificationsCollection(
                new NoChangeRoleSpecification(changedRoles, new DelegateCondition<IEnumerable<string>>(roles => !roles.Contains(Roles.User)))
                );


            var ex = Assert.Throws<NoAccessChangRoleException>(() =>
            {
                specifications.Inspect();
            });


            Assert.NotNull(ex);
            Assert.Equal("Cannot change the role", ex.Message);
        }
    }
}
