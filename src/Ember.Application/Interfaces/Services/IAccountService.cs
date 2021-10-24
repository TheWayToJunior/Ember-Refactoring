using System.Threading.Tasks;
using Ember.Application.Dto;
using Ember.Shared;

namespace Ember.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IResult<AccountDto>> GetAccountAsync(string email);

        Task<IResult> BindAsync(string email, string numberAccount);

        Task<IResult> UnlinkAsync(string email);
    }
}
