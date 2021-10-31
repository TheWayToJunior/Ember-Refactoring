using AutoMapper;
using Ember.Application.Features.News.Queries.GetPage;
using Ember.Application.Interfaces.Data;
using Ember.Application.Mappings;
using Ember.Domain;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ember.Tests
{
    public class GetPageNewsQueryHandlerTests
    {
        private IMapper CreateMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NewsProfile());
            });

            return mapper.CreateMapper();
        }

        [Fact]
        public async Task GetPageNewsQueryHandler_Handle_NewsPagination_CategoryModeAll()
        {
            var query = new GetPageNewsQuery(page: 2, pageSize: 1) { Category = CategoryMode.All };

            var unitOfWork = new Mock<IUnitOfWork<int>>();
            unitOfWork.Setup(u => u.Repository<News>()).Returns(new FakeNewsRepository());

            var handler = new GetPageNewsQueryHandler(unitOfWork.Object, CreateMapper());

            /// Act
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var pagination = Assert.IsType<GetPageNewsResponse>(result.Value);

            Assert.Single(pagination.Values);
            Assert.Equal(3, pagination.TotalSize);
            Assert.Equal(3, pagination.TotalPages);
            Assert.Equal(2, pagination.Page);
        }

        [Fact]
        public async Task GetPageNewsQueryHandler_Handle_NewsPagination_CategoryModeEcology()
        {
            var query = new GetPageNewsQuery(page: 2, pageSize: 1) { Category = CategoryMode.Ecology };

            var unitOfWork = new Mock<IUnitOfWork<int>>();
            unitOfWork.Setup(u => u.Repository<News>()).Returns(new FakeNewsRepository());

            var handler = new GetPageNewsQueryHandler(unitOfWork.Object, CreateMapper());

            /// Act
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var pagination = Assert.IsType<GetPageNewsResponse>(result.Value);

            Assert.Single(pagination.Values);
            Assert.Equal(2, pagination.TotalSize);
            Assert.Equal(2, pagination.TotalPages);
            Assert.Equal(2, pagination.Page);
        }
    }
}
