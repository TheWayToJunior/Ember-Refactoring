using Ember.Domain.Contracts;
using Ember.Shared.Models;
using MediatR;

namespace Ember.Application.Features.Payment.Command.Pay
{
    public class CreatePayCommand : IRequest<IResult>, IPayment
    {
        public СreditСard СreditСard { get; set; }

        public decimal Amount { get; set; }

        public string NumberAccount { get; set; }
    }
}
