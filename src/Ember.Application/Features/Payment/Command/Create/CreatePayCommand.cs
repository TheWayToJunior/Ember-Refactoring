using Ember.Domain.Contracts;
using Ember.Shared;
using MediatR;

namespace Ember.Application.Features.Payment.Command.Pay
{
    public class CreatePayCommand : IRequest<IResult>, IPayment
    {
        public decimal Amount { get; set; }

        public string NumberAccount { get; set; }
    }
}
