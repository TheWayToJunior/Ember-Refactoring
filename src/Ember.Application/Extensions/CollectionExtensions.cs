using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method)
        {
            var results = new List<TResult>();

            foreach (var item in source)
            {
                results.Add(await method(item));
            }

            return results;
        }
    }
}
