using Ember.Domain.Contracts;
using System;

namespace Ember.Domain
{
    public class Accrual : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
