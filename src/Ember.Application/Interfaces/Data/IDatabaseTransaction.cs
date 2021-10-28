using System;

namespace Ember.Application.Interfaces.Data
{
    public interface IDatabaseTransaction : IDisposable
    {
        void Begin();

        void Commit();

        void Rollback();
    }
}
