using AutoMapper;
using Ember.Application.Interfaces;
using Ember.Domain;
using Ember.Domain.Contracts;
using Ember.Infrastructure.Data;
using Ember.Infrastructure.Data.Entitys;
using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Services
{
    public class AccountService : IAccountService
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

        public async Task<IResult<AccountDto>> GetAccountAsync(string email)
        {
            var resultBuilder = OperationResult<AccountDto>.CreateBuilder();

            IUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return resultBuilder.AppendError($"User not found: {email}")
                    .BuildResult();
            }

            UserAccount userAccount = await _context.UsersAccounts
                .Include(ua => ua.Account)
                .Include(ua => ua.Account.Payments)
                .FirstOrDefaultAsync(us => us.UserId.Equals(user.Id));

            if (userAccount is null)
            {
                return resultBuilder.AppendError($"The user: {email} has no binding")
                    .BuildResult();
            }

            return resultBuilder.SetValue(
                _mapper.Map<AccountDto>(userAccount.Account))
                .BuildResult();
        }

        public async Task<IResult> BindAsync(string email, string numberAccount)
        {
            var resultBuilder = OperationResult.CreateBuilder();

            var resultCheckBinding = await CheckBindingAsync(email);

            if (resultCheckBinding.IsSuccess)
            {
                return resultBuilder.AppendErrors(resultCheckBinding.Errors)
                    .AppendError("This Email is already linked to the account number").BuildResult();
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await GetAccountByNumberAsync(numberAccount);

            if (!result.IsSuccess)
            {
                return resultBuilder.AppendErrors(result.Errors).BuildResult();
            }

            Account account = result.Value;

            _context.UsersAccounts.Add(new UserAccount(user, account));

            await _context.SaveChangesAsync();
            return resultBuilder.BuildResult();
        }

        public async Task<IResult> UnlinkAsync(string email)
        {
            var resultBuilder = OperationResult.CreateBuilder();

            var bindingCheack = await CheckBindingAsync(email);

            if (!bindingCheack.IsSuccess)
            {
                return resultBuilder.AppendErrors(bindingCheack.Errors)
                    .AppendError("This Email is already linked to the account number").BuildResult();
            }

            var binding = await _context.UsersAccounts.Include(ua => ua.User)
                .FirstAsync(ua => ua.User.Email.Equals(email));

            _context.UsersAccounts.Remove(binding);
            await _context.SaveChangesAsync();

            return resultBuilder.BuildResult();
        }

        private async Task<IResult<Account>> GetAccountByNumberAsync(string numberAccount)
        {
            var resultBuilder = OperationResult<Account>.CreateBuilder();

            Account account = await _context.Accounts.FirstOrDefaultAsync(a => a.Number.Equals(numberAccount));

            if (account is null)
            {
                return resultBuilder.AppendError($"The account number is incorrect: {numberAccount}")
                    .BuildResult();
            }

            return resultBuilder.SetValue(account)
                .BuildResult();
        }

        private async Task<IResult> CheckBindingAsync(string email)
        {
            var builderResult = OperationResult.CreateBuilder();

            IUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return builderResult.AppendError($"User not found: {email}")
                    .BuildResult();
            }

            var isAny = await _context.UsersAccounts.AnyAsync(ua =>
                ua.UserId.Equals(user.Id));

            if (!isAny)
            {
                return builderResult.AppendError($"The user: {email} is not linked to the account")
                    .BuildResult();
            }

            return builderResult.BuildResult();
        }
    }
}
