using Ember.Application.Interfaces.Data;
using Ember.Domain;
using Ember.Infrastructure.Data;
using Ember.Infrastructure.Data.Repositories;
using Xunit;

namespace Ember.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void UnitOfWork_GetAndCreateRepository()
        {
            ApplicationDbContext context = null;

            IUnitOfWork<int> unitOfWork = new UnitOfWork<int>(context);

            /// Act
            var repository = unitOfWork.Repository<News>();

            Assert.NotNull(repository);
            Assert.Equal(typeof(Repository<News, int>), repository.GetType());
        }
    }
}
