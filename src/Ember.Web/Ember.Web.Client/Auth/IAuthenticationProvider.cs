using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Client.Auth
{
    public interface IAuthenticationProvider
    {
        Task Login(UserToken userToken);

        Task Logout();
    }
}
