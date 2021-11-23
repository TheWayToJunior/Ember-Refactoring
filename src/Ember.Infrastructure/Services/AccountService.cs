using AutoMapper;
using Ember.Application.Interfaces;
using Ember.Application.Interfaces.Services;
using Ember.Domain;
using Ember.Domain.Contracts;
using Ember.Infrastructure.Data;
using Ember.Infrastructure.Data.Entitys;
using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Services
{
    public class AccountService : IAccountService, IAccountPayHistoryService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IResult<AccountDTO>> GetAccountAsync(string email)
        {
            var resultBuilder = OperationResult<AccountDTO>.CreateBuilder();

            IUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return resultBuilder.AppendError($"User not found: {email}")
                    .BuildResult();
            }

            UserAccount userAccount = await _context.UsersAccounts
                .Include(ua => ua.Account)
                    .Include(ua => ua.Account.Payments.OrderByDescending(p => p.Date))
                    .Include(ua => ua.Account.Accruals.OrderByDescending(p => p.Date))
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(us => us.UserId.Equals(user.Id));

            if (userAccount is null)
            {
                return resultBuilder.AppendError($"The user: {email} has no binding")
                    .BuildResult();
            }

            var result = _mapper.Map<AccountDTO>(userAccount.Account);
            result.Amount = await CalculateAmountAsync(userAccount.Account);

            return resultBuilder.SetValue(result)
                .BuildResult();
        }

        private async Task<decimal> CalculateAmountAsync(Account account)
        {
            var accruals = await Task.Run(() => account.Accruals.Sum(a => a.Amount));
            var payments = await Task.Run(() => account.Payments.Sum(a => a.Amount));

            return payments - accruals;
        }

        public async Task<IResult> BindAsync(string email, string numberAccount)
        {
            var resultBuilder = OperationResult.CreateBuilder();

            var notLinked = await IsNotLinkedAsync(email);

            if (!notLinked.IsSuccess)
            {
                return resultBuilder.AppendErrors(notLinked.Errors).BuildResult();
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await GetAccountByNumberAsync(numberAccount);

            if (!result.IsSuccess)
            {
                return resultBuilder.AppendErrors(result.Errors).BuildResult();
            }

            Account account = result.Value;

            _context.UsersAccounts.Add(new UserAccount(user, account));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultBuilder.AppendError($"Message: {ex.Message}; InnerException: {ex.InnerException.Message}");
            }

            return resultBuilder.BuildResult();
        }

        public async Task<IResult> UnlinkAsync(string email)
        {
            var resultBuilder = OperationResult.CreateBuilder();

            var notLinked = await IsNotLinkedAsync(email);

            if (notLinked.IsSuccess)
            {
                return resultBuilder.AppendErrors(notLinked.Errors).BuildResult();
            }

            var binding = await _context.UsersAccounts.Include(ua => ua.User)
                .FirstAsync(ua => ua.User.Email.Equals(email));

            _context.UsersAccounts.Remove(binding);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultBuilder.AppendError($"Message: {ex.Message}; InnerException: {ex.InnerException.Message}");
            }

            return resultBuilder.BuildResult();
        }

        private async Task<IResult<Account>> GetAccountByNumberAsync(string numberAccount)
        {
            var resultBuilder = OperationResult<Account>.CreateBuilder();

            Account account = await _context.Accounts
                .Include(a => a.Payments)
                .FirstOrDefaultAsync(a => a.Number.Equals(numberAccount));

            if (account is null)
            {
                return resultBuilder.AppendError($"The account number is incorrect: {numberAccount}")
                    .BuildResult();
            }

            return resultBuilder.SetValue(account)
                .BuildResult();
        }

        private async Task<IResult> IsNotLinkedAsync(string email)
        {
            var resultBuilder = OperationResult.CreateBuilder();

            IUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return resultBuilder.AppendError($"User not found: {email}")
                    .BuildResult();
            }

            var isAny = await _context.UsersAccounts.AnyAsync(ua =>
                ua.UserId.Equals(user.Id));

            if (isAny)
            {
                return resultBuilder.AppendError($"The user: {email} is already linked to the account number")
                    .BuildResult();
            }

            return resultBuilder.BuildResult();
        }

        public async Task<IResult> AddPayHistoryAsync(IReceipt receipt)
        {
            var resultBuilder = OperationResult.CreateBuilder();
            var result = await GetAccountByNumberAsync(receipt.NumberAccount);

            if (!result.IsSuccess)
            {
                return resultBuilder.AppendErrors(result.Errors)
                    .BuildResult();
            }

            Account account = result.Value;
            account.Payments.Add(new Payment
            {
                Amount = receipt.Amount,
                AccountId = account.Id,
                Account = account,
                Date = DateTime.Now
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultBuilder.AppendError($"Message: {ex.Message}; InnerException: {ex.InnerException?.Message}");
            }

            return resultBuilder.BuildResult();
        }
    }
}
