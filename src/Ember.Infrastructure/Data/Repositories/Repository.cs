using Ember.Application.Extensions;
using Ember.Application.Interfaces.Data;
using Ember.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Data.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetPageAsync(int page, int pageSize)
        {
            return await _context.Set<TEntity>()
                .GetPage(page, pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _context.Set<TEntity>()
                .FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>()
                .AddAsync(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            TEntity exist = _context.Set<TEntity>()
                .Find(entity.Id);

            _context.Entry(exist).CurrentValues.SetValues(entity);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>()
                .Remove(entity);

            return Task.CompletedTask;
        }
    }
}