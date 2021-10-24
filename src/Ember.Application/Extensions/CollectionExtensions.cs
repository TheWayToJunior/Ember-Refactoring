using System.Linq;

namespace Ember.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static IQueryable<T> GetPage<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            return queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
