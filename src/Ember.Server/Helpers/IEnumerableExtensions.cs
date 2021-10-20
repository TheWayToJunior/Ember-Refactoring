using Ember.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Pagination<T>(this IEnumerable<T> enumerable, PaginationDTO pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            return enumerable
                .Skip((pagination.Page - 1) * pagination.QuantityPerPage)
                .Take(pagination.QuantityPerPage);
        }

        public static IQueryable<T> Pagination<T>(this IQueryable<T> queryable, PaginationDTO pagination) 
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            return queryable
                .Skip((pagination.Page - 1) * pagination.QuantityPerPage)
                .Take(pagination.QuantityPerPage);
        }
    }
}
