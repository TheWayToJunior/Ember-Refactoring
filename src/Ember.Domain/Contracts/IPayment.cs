namespace Ember.Domain.Contracts
{
    public interface IPayment
    {
        decimal Amount { get; set; }

        string NumberAccount { get; set; }
    }
}
