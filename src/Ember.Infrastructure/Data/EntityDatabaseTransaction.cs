using Ember.Application.Interfaces.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ember.Infrastructure.Data
{
    public class EntityDatabaseTransaction : IDatabaseTransaction
    {
        private bool _disposed;

        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public EntityDatabaseTransaction(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Begin()
        {
            if (_transaction != null)
            {
                throw new ValidationException("Already opened");
            }

            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
            {
                throw new ValidationException($"There is no transaction");
            }

            _transaction.Commit();
        }

        public void Rollback()
        {
            if (_transaction == null)
            {
                throw new ValidationException("There is no open transaction");
            }

            _transaction.Rollback();
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
