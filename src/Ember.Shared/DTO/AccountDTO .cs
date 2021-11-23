using System.Collections.Generic;

namespace Ember.Shared
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public decimal Amount { get; set; }

        public string Address { get; set; }

        public virtual IEnumerable<PaymentDTO> Payments { get; set; }

        public virtual IEnumerable<AccrualDTO> Accruals { get; set; }
    }
}
