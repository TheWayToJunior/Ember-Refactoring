using Ember.Server.Data;
using Ember.Server.Exceptions;
using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ember.Server.Services
{
    public class BindAccountService : IBindAccountService
    {
        private readonly ApplicationDbContext context;

        public BindAccountService(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Account> GetAccount(string userId)
        {
            var userAccount = await context.UsersAccounts
                .Include(ua => ua.Account)
                .Include(ua => ua.Account.PaymentHistories)
                .FirstAsync(us => us.UserId == userId)
                .ConfigureAwait(true);

            return userAccount.Account;
        }

        private async Task<Account> GetAccountByNumberAsync(string numberAccount)
        {
            var account = await context.Accounts.FirstOrDefaultAsync(a => a.Number == numberAccount)
                .ConfigureAwait(true);

            if (account == null)
            {
                throw new NoSpecifiedElementException("The specified personal account was not found");
            }

            return account;
        }

        public async Task Bind(IdentityUser user, string numberAccount)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var account = await GetAccountByNumberAsync(numberAccount)
                .ConfigureAwait(true);

            context.UsersAccounts.Add(new UserAccount
            {
                UserId = user.Id,
                User = user,
                AccountId = account.Number,
                Account = account
            });

            await context.SaveChangesAsync()
                .ConfigureAwait(true);
        }

        public async Task Unlink(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var account = await GetAccount(user.Id)
                .ConfigureAwait(true);

            var userAccount = await context.UsersAccounts.FindAsync(user.Id, account.Number)
               .ConfigureAwait(true);

            if (userAccount == null)
            {
                throw new NoSpecifiedElementException("The specified binding was not found");
            }

            context.UsersAccounts.Remove(userAccount);

            await context.SaveChangesAsync()
              .ConfigureAwait(true);
        }

        public Task<bool> CheckBinding(IdentityUser user)
        {
            return context.UsersAccounts.AnyAsync(ua => ua.UserId == user.Id);
        }
    }
}
