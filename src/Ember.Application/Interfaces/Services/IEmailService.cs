using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendMessage(MessageRequest message);
    }
}
