using Ember.Application.Interfaces.Data;
using Ember.Shared;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using NewsEntity = Ember.Domain.News;

namespace Ember.Application.Features.News.Commands.Delete
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, IResult>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteNewsCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var resultBuilder = OperationResult.CreateBuilder();
            var repository = _unitOfWork.Repository<NewsEntity>();

            var entity = await repository.GetByIdAsync(request.Id);

            if(entity is null)
            {
                return resultBuilder
                    .AppendError($"Couldn't find the news by the specified id: {request.Id}")
                    .BuildResult();
            }

            await repository.DeleteAsync(entity);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                resultBuilder.AppendError(ex.Message)
                    .AppendError(ex.InnerException?.Message);
            }

            return resultBuilder.BuildResult();
        }
    }
}
