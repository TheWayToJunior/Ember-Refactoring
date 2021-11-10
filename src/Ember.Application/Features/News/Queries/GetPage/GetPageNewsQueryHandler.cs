using AutoMapper;
using Ember.Application.Extensions;
using Ember.Application.Interfaces.Data;
using Ember.Domain;
using Ember.Domain.Contracts;
using Ember.Shared;
using Ember.Shared.Models;
using LinqKit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IResult<GetPageNewsResponse>> Handle(GetPageNewsQuery request, CancellationToken cancellationToken)
        {
            var resultBulder = OperationResult<GetPageNewsResponse>.CreateBuilder();
            var repository = _unitOfWork.Repository<NewsEntity>();

            var filtered = repository.GetAll()
                .Where(CreatePredicate(request.Category));

            var query = filtered
                .OrderByDescending(news => news.Time)
                .GetPage(request.Page, request.PageSize);

            var entities = await Task.Run(() => query.ToList());

            var response = new GetPageNewsResponse()
            {
                Values = _mapper.Map<IEnumerable<NewsDTO>>(entities),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalSize = await Task.Run(() => filtered.Count())
            };

            return await Task.FromResult(resultBulder.SetValue(response).BuildResult());
        }

        private Expression<Func<NewsEntity, bool>> CreatePredicate(CategoryMode mode)
        {
            var predicate = PredicateBuilder.New<NewsEntity>(true);

            if (mode != CategoryMode.All)
            {
                Expression<Func<NewsEntity, bool>> conditions = news =>
                    news.Category.Equals(mode);

                predicate.And(conditions);
            }

            return predicate;
        }
    }
}
