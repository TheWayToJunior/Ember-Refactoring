using Ember.Application.Extensions;
using Ember.Application.Interfaces.Data;
using Ember.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Tests
{
    internal class FakeNewsRepository : IRepository<News, int>
    {
        private ICollection<News> _news;

        public FakeNewsRepository()
        {
            _news = new List<News>
            {
                new News{ Id = 1, Category = CategoryMode.Ecology, Title = "1", Time = System.DateTime.Now, Description = "1"},
                new News{ Id = 2, Category = CategoryMode.Ecology, Title = "2", Time = System.DateTime.Now, Description = "2"},
                new News{ Id = 3, Category = CategoryMode.Repair,  Title = "3", Time = System.DateTime.Now, Description = "3"}
            };
        }

        public Task DeleteAsync(News entity)
        {
            _news.Remove(entity);
            return Task.CompletedTask;
        }

        public IQueryable<News> GetAll()
        {
            return _news.AsQueryable();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await Task.FromResult(_news
                .Single(n => n.Id.Equals(id)));
        }

        public async Task<IEnumerable<News>> GetPageAsync(int page, int pageSize)
        {
            return await Task.FromResult(_news
                .AsQueryable()
                .GetPage(page, pageSize)
                .ToList());
        }

        public Task InsertAsync(News entity)
        {
            _news.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(News entity)
        {
            var list = _news as List<News>;
            var index = list.FindIndex(0, _news.Count, n => n.Id == entity.Id);

            list.RemoveAt(index);
            list.Insert(index, entity);

            return Task.CompletedTask;
        }
    }
}