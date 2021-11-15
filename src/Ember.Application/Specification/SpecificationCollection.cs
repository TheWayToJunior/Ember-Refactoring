using System.Collections.Generic;

namespace Ember.Application.Specification
{
    public class SpecificationsCollection
    {
        private IEnumerable<ISpecification> _rules;

        public SpecificationsCollection(params ISpecification[] rules)
        {
            _rules = rules;
        }

        public void Inspect()
        {
            foreach (var rule in _rules)
            {
                rule.CheckExecution();
            }
        }
    }
}
