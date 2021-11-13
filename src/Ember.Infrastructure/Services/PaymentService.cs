using Ember.Application.Interfaces;
using Ember.Domain.Contracts;
using Ember.Shared;
using System;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Services
{
    /// <summary>
    /// Fake payment service
    /// </summary>
    public class PaymentService : IPaymentService
    {
        public async Task<IResult<IReceipt>> ToPayAsync(IPayment payment)
        {
            var resultBuilder = OperationResult<Receipt>.CreateBuilder();

            var receipt = new Receipt()
            {
                Id = Guid.NewGuid(),
                Amount = payment.Amount,
                NumberAccount = payment.NumberAccount,
                Date = DateTime.Now,
                BankAccount = "Bank payment account"
            };

            return await Task.FromResult(resultBuilder.SetValue(receipt).BuildResult());
        }
    }
}
