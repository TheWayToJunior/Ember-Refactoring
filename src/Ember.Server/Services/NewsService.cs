using Ember.Server.Data;
using Ember.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext context;

        public NewsService(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<NewsPost> GetAll() => context.Posts;

        public async Task<NewsPost> GetById(int id) => await context.Posts.FindAsync(id);

        public async Task<NewsPost> AddAsync(NewsPost post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            var saved = await context.Posts.AddAsync(post);

            await context.SaveChangesAsync()
                .ConfigureAwait(true);

            return saved.Entity as NewsPost;
        }

        public async Task UpdateAsync(NewsPost post) 
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            try
            {
                context.Entry(post).State = EntityState.Modified;

                await context.SaveChangesAsync()
                    .ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw new DbUpdateConcurrencyException();
            }
        }

        public async Task DeleteAsync(NewsPost post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            context.Remove(post);

            await context.SaveChangesAsync()
                .ConfigureAwait(true);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await context.Posts.AnyAsync(post => post.Id == id)
                .ConfigureAwait(true);
        }
    }
}
