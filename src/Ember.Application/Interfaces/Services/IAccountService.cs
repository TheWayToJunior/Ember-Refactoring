using Ember.Domain.Contracts;
using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IResult<AccountDto>> GetAccountAsync(string email);

        Task<IResult> BindAsync(string email, string numberAccount);

        Task<IResult> UnlinkAsync(string email);
    }
}
