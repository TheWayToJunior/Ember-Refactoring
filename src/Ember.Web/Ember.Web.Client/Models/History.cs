using System;

namespace Ember.Web.Client.Models
{
    /// <summary>
    /// Temporary class for displaying the user's payment history
    /// </summary>
    public class History
    {
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public bool IsSuccess { get; set; }
    }
}
