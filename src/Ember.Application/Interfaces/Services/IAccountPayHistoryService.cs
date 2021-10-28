using Ember.Domain.Contracts;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces.Services
{
    public interface IAccountPayHistoryService
    {
        Task<IResult> AddPayHistoryAsync(IReceipt receipt);
    }
}
