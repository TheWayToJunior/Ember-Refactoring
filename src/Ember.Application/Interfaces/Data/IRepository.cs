using Ember.Domain.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces.Data
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetPageAsync(int page, int pageSize);

        Task<TEntity> GetByIdAsync(TKey id);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
