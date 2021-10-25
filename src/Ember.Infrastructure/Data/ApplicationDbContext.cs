using Ember.Domain;
using Ember.Infrastructure.Data.Entities;
using Ember.Infrastructure.Data.Entitys;
using Ember.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ember.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole, 
        string,
        IdentityUserClaim<string>,
        ApplicationUserRole,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
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
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            DataInitializer.Initialize(builder);
        }
    }
}