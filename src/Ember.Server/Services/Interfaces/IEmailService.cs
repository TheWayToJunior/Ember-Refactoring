using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Server
{
    public interface IEmailService
    {
        Task SendMessage(SendMessage message);
    }
}
