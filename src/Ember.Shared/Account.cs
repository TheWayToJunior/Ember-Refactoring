using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ember.Shared
{
    public class Account
    {
        [Key]
        [StringLength(6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Number { get; set; }

        public decimal Payment { get; set; }

        public string Address { get; set; }

        public virtual List<PaymentHistory> PaymentHistories { get; set; }
    }

    public class PaymentHistory
    {
        public int Id { get; set; }

        public decimal Payment { get; set; }

        public DateTime Date { get; set; }

        public virtual Account Account { get; set; }
    }
}
