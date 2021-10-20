using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Ember.Server
{
    public class Initialization
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { Roles.Admin, Roles.Editor, Roles.User };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName)
                    .ConfigureAwait(true);

                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName))
                        .ConfigureAwait(true);
                }
            }

            // find the user with the admin email 
            var user = await UserManager.FindByEmailAsync("admin@email.com")
                        .ConfigureAwait(true);

            // check if the user exists
            if (user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var poweruser = new IdentityUser
                {
                    UserName = "admin@email.com",
                    Email = "admin@email.com",
                };
                string adminPassword = "Miha1932";

                var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword)
                        .ConfigureAwait(true);

                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin")
                        .ConfigureAwait(true);
                }
            }
        }
    }
}
