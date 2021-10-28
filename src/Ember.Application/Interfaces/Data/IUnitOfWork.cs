using Ember.Domain.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces.Data
{
    public interface IUnitOfWork<TKey>
    {
        public IRepository<TEntity, TKey> Repository<TEntity>()
            where TEntity : class, IEntity<TKey>;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task Rollback();

        IDatabaseTransaction BeginTransaction();
    }
}
