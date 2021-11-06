using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Client.Auth
{
    public interface ILoginService
    {
        Task Login(UserToken userToken);

        Task Logout();
    }
}
