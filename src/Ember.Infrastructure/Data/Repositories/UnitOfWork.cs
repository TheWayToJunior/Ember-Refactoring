using Ember.Application.Interfaces.Data;
using Ember.Domain.Contracts;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Data.Repositories
{
    public class UnitOfWork<TKey> : IUnitOfWork<TKey>, IDisposable
    {
        private readonly ApplicationDbContext _context;

        private Hashtable _repositories;

        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity, TKey> Repository<TEntity>()
            where TEntity : class, IEntity<TKey>
        {
            if (_repositories is null)
            {
                _repositories = new();
            }

            string type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositotyType = typeof(Repository<,>);
                var repositotyInstance = Activator.CreateInstance(
                    repositotyType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);

                _repositories.Add(type, repositotyInstance);
            }

            return _repositories[type] as IRepository<TEntity, TKey>;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public virtual IDatabaseTransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(_context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
