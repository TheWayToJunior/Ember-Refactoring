using Ember.Domain;
using Ember.Infrastructure.Data.Entitys;
using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Helpers
{
    public class DataInitializer
    {
        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<ApplicationRole> _roleManager;
        private static ILogger<DataInitializer> _logger;

        private static async Task CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));

            if(!result.Succeeded)
            {
                _logger.LogError($"Error: {string.Join("\n", result.Errors.Select(e => e.Description))}");
            }

            _logger.LogInformation($"Created role: {roleName}");
        }

        private static async Task CreateAdminAsync()
        {
            var poweruser = new ApplicationUser(email: "admin@email.com", userName: "admin@email.com");
            string adminPassword = "Miha1932";

            var result = await _userManager.CreateAsync(poweruser, adminPassword);

            if (!result.Succeeded)
            {
                _logger.LogError($"Error: {string.Join("\n", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRolesAsync(poweruser, new[] { Roles.Admin, Roles.User });
            _logger.LogInformation($"Admin created: {poweruser.UserName}");
        }

        public static async Task IdentityInitializeAsync(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            _logger = serviceProvider.GetRequiredService<ILogger<DataInitializer>>();


            /// Create roles
            string[] roleNames = { Roles.Admin, Roles.Editor, Roles.Consumer, Roles.User };
            _logger.LogInformation("Checking role registration");

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    await CreateRoleAsync(roleName);
                }
            }

            /// Create user
            var user = await _userManager.FindByEmailAsync("admin@email.com");
            _logger.LogInformation("Checking administrator account registration");

            if (user == null)
            {
                await CreateAdminAsync();
            }
        }

        public static void Initialize(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Entity<News>().HasData(
                new News[]
                    {
                        new News
                        {
                            Id = 1,
                            Time = DateTime.Now,
                            ImageSrc="https://sun9-9.userapi.com/c850128/v850128254/1d36a9/B54sYaowd5E.jpg",
                            Title= "Об итогах ремонтного периода.",
                            Description = "Согласно Правил подготовки теплового хозяйства к отопительному сезону предприятием были" +
                                          " разработаны мероприятия по подготовке объектов теплоснабжения к работе в осеннее-зимний",
                            Category = CategoryMode.Repair
                        },

                        new News
                        {
                            Id = 2,
                            Time = DateTime.Now,
                            ImageSrc="https://sun9-28.userapi.com/c204516/v204516299/3b411/0qjhwQo15mw.jpg",
                            Title= "Внимание произвадятся работы!!!",
                            Description = "Согласно Правил подготовки теплового хозяйства к отопительному сезону предприятием были" +
                                          " разработаны мероприятия по подготовке объектов теплоснабжения к работе в осеннее-зимний",
                            Category =  CategoryMode.Repair
                        },

                        new News
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

            var accounts = new Account[]
            {
                    new Account
                    {
                        Id = 1,
                        Number = "193216",
                        Address = "ул. Великан д. 21 кв. 28"
                    },
                    new Account
                    {
                        Id = 2,
                        Number = "321619",
                        Address = "ул. Жарова д. 5а кв. 47"
                    },
                    new Account
                    {
                        Id = 3,
                        Number = "161932",
                        Address = "ул. Нежская д. 19"
                    }
            };

            builder.Entity<Account>().HasData(accounts);

            builder.Entity<Payment>().HasData(
                new Payment[]
                {
                    new Payment
                    {
                        Id = 1,
                        AccountId = 1,
                        Amount = 1300,
                        Date = DateTime.Now
                    },

                    new Payment
                    {
                        Id = 2,
                        AccountId = 2,
                        Amount = 750,
                        Date = DateTime.Now.AddDays(-20)
                    },
                });

            builder.Entity<Accrual>().HasData(
                new Accrual[]
                {
                    new Accrual
                    {
                        Id = 1,
                        AccountId = 1,
                        Amount = 1250.68m,
                        Date = DateTime.Now
                    },

                    new Accrual
                    {
                        Id = 2,
                        AccountId = 2,
                        Amount = 1050.68m,
                        Date = DateTime.Now
                    },

                    new Accrual
                    {
                        Id = 3,
                        AccountId = 3,
                        Amount = 1250.68m,
                        Date = DateTime.Now
                    },

                    new Accrual
                    {
                        Id = 4,
                        AccountId = 1,
                        Amount = 730.68m,
                        Date = DateTime.Now.AddMonths(1)
                    },

                    new Accrual
                    {
                        Id = 5,
                        AccountId = 2,
                        Amount = 795.15m,
                        Date =DateTime.Now.AddMonths(1)
                    },

                    new Accrual
                    {
                        Id = 6,
                        AccountId = 3,
                        Amount = 942.50m,
                        Date = DateTime.Now.AddMonths(1)
                    }
                });
        }
    }
}
