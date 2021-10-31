using System;
using MediatR;
using Ember.Shared;
using System.Threading;
using System.Threading.Tasks;
using Ember.Domain.Contracts;
using Ember.Application.Interfaces;
using Ember.Application.Interfaces.Data;
using Ember.Application.Interfaces.Services;

namespace Ember.Application.Features.Payment.Command.Pay
{
    public class CreatePayCommandHandler : IRequestHandler<CreatePayCommand, IResult>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAccountPayHistoryService _accountService;
        private readonly IPaymentService _paymentService;

        public CreatePayCommandHandler(IUnitOfWork<int> unitOfWork, IAccountPayHistoryService accountService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _paymentService = paymentService;
        }

        public async Task<IResult> Handle(CreatePayCommand request, CancellationToken cancellationToken)
        {
            var resultBuilder = OperationResult.CreateBuilder();

            /// First, we save the payment data to the database, and then we make the payment.
            /// Thus, in case of payment failure, we will be able to roll back the saves in the database.
            using var transaction = _unitOfWork.BeginTransaction();
            transaction.Begin();

            var initialReceipt = new Receipt(request.Amount, request.NumberAccount);

            try
            {
                await AddPayHistoryAsync(initialReceipt);
                await ToPayAsync(request);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                resultBuilder.AppendError($"Message: {ex.Message}").
                    AppendError($"InnerException: {ex.InnerException.Message}");
            }

            return resultBuilder.BuildResult();
        }

        private async Task AddPayHistoryAsync(IReceipt receipt)
        {
            var result = await _accountService.AddPayHistoryAsync(receipt);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(
                    "Payment receipt could not be saved", new Exception(string.Join("; ", result.Errors)));
            }
        }

        private async Task ToPayAsync(IPayment payment)
        {
            var result = await _paymentService.ToPayAsync(payment);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(
                    "The payment operation failed", new Exception(string.Join("; ", result.Errors)));
            }
        }
    }
}
