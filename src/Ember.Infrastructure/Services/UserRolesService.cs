using Ember.Application.Interfaces.Services;
using Ember.Infrastructure.Data;
using Ember.Infrastructure.Data.Entitys;
using Ember.Shared;
using Ember.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ApplicationDbContext _context;

        public UserRolesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResult<PaginationResponse<UserRolesDto>>> GetPageUsersWithRolesAsync(PaginationRequest request, string roleFilter)
        {
            //IEnumerable<ApplicationUser> users = string.IsNullOrEmpty(roleFilter)
            //    ? await _userManager.Users.ToListAsync()
            //    : (await _userManager.GetUsersInRoleAsync(roleFilter)).AsQueryable();

            //var role = await _roleManager.FindByNameAsync(roleFilter);
            //var users = role.UserRoles.Select(ur => ur.User);

            throw new NotImplementedException();
        }

        private async Task<UserRolesDto> CreateUserRoles(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            return new UserRolesDto(user.Email, roles.Select(r => r.Name), userRoles);
        }
    }
}
