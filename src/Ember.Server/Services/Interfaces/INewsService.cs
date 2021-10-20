using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server
{
    /// <summary>
    /// The repository interface
    /// </summary>
    public interface INewsService
    {
        IQueryable<NewsPost> GetAll();

        Task<NewsPost> GetById(int id);

        Task<NewsPost> AddAsync(NewsPost post);

        Task UpdateAsync(NewsPost post);

        Task DeleteAsync(NewsPost post);

        Task<bool> AnyAsync(int id);
    }
}
