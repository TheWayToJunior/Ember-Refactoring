using AutoMapper;
using Ember.Application.Extensions;
using Ember.Application.Interfaces.Data;
using Ember.Domain;
using Ember.Shared;
using Ember.Shared.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NewsEntity = Ember.Domain.News;

namespace Ember.Application.Features.News.Queries.GetPage
{
    public class GetPageNewsQueryHandler : IRequestHandler<GetPageNewsQuery, IResult<GetPageNewsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPageNewsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// TODO: Redo the news categories, and the selection algorithm for them
        public async Task<IResult<GetPageNewsResponse>> Handle(GetPageNewsQuery request, CancellationToken cancellationToken)
        {
            var resultBulder = OperationResult<GetPageNewsResponse>.CreateBuilder();
            var repository = _unitOfWork.Repository<NewsEntity>();

            var postasDescending = repository.GetAll().OrderByDescending(news => news.Id);

            IQueryable<NewsEntity> posts =   !request.Category.Equals(CategoryMode.All)
                ? postasDescending.Where(news => news.Category.Equals(request.Category))
                : postasDescending;

            var page = posts.GetPage(request.Page, request.PageSize).ToList();

            var response = new GetPageNewsResponse()
            {
                Values = _mapper.Map<IEnumerable<NewsDto>>(page),
                Page = request.Page,
                PageSize = request.PageSize
            };

            return await Task.FromResult(resultBulder.SetValue(response).BuildResult());
        }
    }
}
