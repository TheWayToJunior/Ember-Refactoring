using Ember.Domain.Contracts;
using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<IResult<UserTokenResponse>> RegistrationAsync(AuthenticationReques reques);

        Task<IResult<UserTokenResponse>> LoginAsync(AuthenticationReques reques);
    }
}