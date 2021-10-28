using System.Collections.Generic;

namespace Ember.Shared
{
    public class AccountDto 
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public decimal Payment { get; set; }

        public string Address { get; set; }

        public virtual IEnumerable<PaymentDto> Payments { get; set; }
    }
}
