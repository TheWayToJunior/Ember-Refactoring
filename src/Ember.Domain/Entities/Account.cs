using Ember.Domain.Contracts;
using System.Collections.Generic;

namespace Ember.Domain
{
    public class Account : IEntity<int>
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Accrual> Accruals { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
