using Ember.Domain.Contracts;
using System;

namespace Ember.Shared
{
    public class Receipt : IReceipt
    {
        public Receipt()
        {
        }

        public Receipt(decimal amount, string numberAccount)
        {
            Amount = amount;
            NumberAccount = numberAccount;
            Date = DateTime.Now;
        }

        public Guid? Id { get; set; }

        public decimal Amount { get; set; }

        public string NumberAccount { get; set; }

        public DateTime Date { get; set; }

        public string BankAccount { get; set; }
    }
}
