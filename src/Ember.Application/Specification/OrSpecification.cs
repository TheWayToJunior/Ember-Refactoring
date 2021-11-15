using System.Collections.Generic;

namespace Ember.Application.Specification
{
    public class OrSpecification : ISpecification
    {
        private readonly IEnumerable<ISpecification> _specifications;

        public OrSpecification(params ISpecification[] specifications)
        {
            _specifications = specifications;
        }

        public void CheckExecution()
        {
            foreach (var specification in _specifications)
            {
                specification.CheckExecution();
            }
        }
    }
}
