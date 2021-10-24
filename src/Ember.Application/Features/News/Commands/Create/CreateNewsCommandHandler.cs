using AutoMapper;
using Ember.Application.Interfaces.Data;
using Ember.Shared;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using NewsEntity = Ember.Domain.News;

namespace Ember.Application.Features.News.Commands.Create
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, IResult>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public CreateNewsCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var resultBuilder = OperationResult.CreateBuilder();
            var repository = _unitOfWork.Repository<NewsEntity>();

            var entity = _mapper.Map<NewsEntity>(request);
            await repository.InsertAsync(entity);

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
