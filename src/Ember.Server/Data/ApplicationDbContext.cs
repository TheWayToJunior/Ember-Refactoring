using Ember.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ember.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<NewsPost> Posts { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<UserAccount> UsersAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Entity<UserAccount>().HasKey(ua => new { ua.UserId, ua.AccountId });

            builder.Entity<Account>().HasData(
                new Account[]
                {
                    new Account
                    {
                        Number = "193216",
                        Payment = 125,
                        Address = "ул. Великан д. 21 кв. 28"
                    },

                    new Account
                    {
                        Number = "321619",
                        Payment = 75,
                        Address = "ул. Жарова д. 5а кв. 47"
                    },

                    new Account
                    {
                        Number = "161932",
                        Payment = 547,
                        Address = "ул. Нежская д. 19"
                    }
                });

            builder.Entity<NewsPost>().HasData(
                    new NewsPost[]
                    {
                        new NewsPost
                        {
                            Id = 1,
                            Time = DateTime.Now,
                            ImageSrc="https://sun9-9.userapi.com/c850128/v850128254/1d36a9/B54sYaowd5E.jpg",
                            Title= "Об итогах ремонтного периода.",
                            Description = "Согласно Правил подготовки теплового хозяйства к отопительному сезону предприятием были" +
                                          " разработаны мероприятия по подготовке объектов теплоснабжения к работе в осеннее-зимний",
                            Category = CategoryMode.Repair
                        },

                        new NewsPost
                        {
                            Id = 2,
                            Time = DateTime.Now,
                            ImageSrc="https://sun9-28.userapi.com/c204516/v204516299/3b411/0qjhwQo15mw.jpg",
                            Title= "Внимание произвадятся работы!!!",
                            Description = "Согласно Правил подготовки теплового хозяйства к отопительному сезону предприятием были" +
                                          " разработаны мероприятия по подготовке объектов теплоснабжения к работе в осеннее-зимний",
                            Category =  CategoryMode.Repair
                        },

                        new NewsPost
                        {
                            Id = 3,
                            Time = DateTime.Now,
                            ImageSrc="https://sun9-35.userapi.com/c851028/v851028124/196804/0j89FAqJ5Wg.jpg",
                            Title= "Инвестиционная программа 2019 года",
                            Description = "Согласно Правил подготовки теплового хозяйства к отопительному сезону предприятием были" +
                                          " разработаны мероприятия по подготовке объектов теплоснабжения к работе в осеннее-зимний",
                            Category =  CategoryMode.Ecology
                        }
                    });

            base.OnModelCreating(builder);
        }
    }
}
