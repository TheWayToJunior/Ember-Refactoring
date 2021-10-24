using Ember.Domain;

namespace Ember.Infrastructure.Data.Entitys
{
    public class UserAccount
    {
        public UserAccount()
        {
        }

        public UserAccount(ApplicationUser user, Account account)
        {
            User = user;
            UserId = user.Id;

            Account = account;
            AccountId = account.Id;
        }

        public string UserId { get; set; }

        public int AccountId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Account Account { get; set; }
    }
}
