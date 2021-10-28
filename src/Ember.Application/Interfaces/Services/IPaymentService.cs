using Ember.Domain.Contracts;
using Ember.Shared;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<IResult<IReceipt>> ToPayAsync(IPayment payment);
    }
}
