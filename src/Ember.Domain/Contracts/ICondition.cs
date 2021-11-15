namespace Ember.Domain.Contracts
{
    public interface ICondition<T>
    {
        bool IsMatch(T value);
    }
}
