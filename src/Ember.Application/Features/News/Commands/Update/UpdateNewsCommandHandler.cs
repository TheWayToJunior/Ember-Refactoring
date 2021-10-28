using AutoMapper;
using Ember.Application.Interfaces.Data;
using Ember.Domain.Contracts;
using Ember.Shared;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using NewsEntity = Ember.Domain.News;

namespace Ember.Application.Features.News.Commands.Update
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, IResult>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateNewsCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var resultBuilder = OperationResult.CreateBuilder();
            var repository = _unitOfWork.Repository<NewsEntity>();

            var entity = await repository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                return resultBuilder
                    .AppendError($"Update error. The entity could not be found by the specified id: {request.Id}")
                    .BuildResult();
            }

            _mapper.Map(request, entity);
            await repository.UpdateAsync(entity);

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
