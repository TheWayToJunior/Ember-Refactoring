namespace Ember.Domain.Contracts
{
    public interface IReceipt
    {
        decimal Amount { get; }

        string NumberAccount { get; }
    }
}
