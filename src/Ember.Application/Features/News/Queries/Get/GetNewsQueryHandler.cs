using AutoMapper;
using Ember.Application.Dto;
using Ember.Application.Interfaces.Data;
using Ember.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NewsEntity = Ember.Domain.News;

namespace Ember.Application.Features.News.Queries
{
    public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, IResult<GetNewsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetNewsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<GetNewsResponse>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            var resultBuilder = OperationResult<GetNewsResponse>.CreateBuilder();
            var repository = _unitOfWork.Repository<NewsEntity>();

            var entity = await repository.GetByIdAsync(request.Id);

            if(entity is null)
            {
                resultBuilder.AppendError($"Couldn't find the news by the specified id: {request.Id}");
            }

            var response = _mapper.Map<GetNewsResponse>(entity);
            return resultBuilder.SetValue(response).BuildResult();
        }
    }
}
