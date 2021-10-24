using Ember.Domain;
using Ember.Infrastructure.Data.Entitys;
using Ember.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ember.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<News> Posts { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<UserAccount> UsersAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            DataInitializer.Initialize(builder);
            base.OnModelCreating(builder);
        }
    }
}