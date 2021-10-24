using Ember.Application.Interfaces;
using Ember.Application.Interfaces.Data;
using System;

namespace Ember.Server.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public PaymentService(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void ToPay()
        {

        }
    }
}
