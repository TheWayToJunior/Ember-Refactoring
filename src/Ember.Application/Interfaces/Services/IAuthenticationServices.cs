using Ember.Domain.Contracts;
using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<IResult<UserToken>> RegistrationAsync(AuthenticationReques reques);

        Task<IResult<UserToken>> LoginAsync(AuthenticationReques reques);
    }
}