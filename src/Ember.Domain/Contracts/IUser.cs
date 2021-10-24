namespace Ember.Domain.Contracts
{
    public interface IUser : IEntity<string>
    {
        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
