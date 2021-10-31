using Ember.Application.Extensions;
using Ember.Application.Interfaces.Services;
using Ember.Domain.Contracts;
using Ember.Infrastructure.Data;
using Ember.Infrastructure.Data.Entities;
using Ember.Infrastructure.Data.Entitys;
using Ember.Server.Exceptions;
using Ember.Shared;
using Ember.Shared.Responses;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ApplicationDbContext _context;

        public UserRolesService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _context = context;
        }

        public async Task<IResult<PaginationResponse<UserRolesDto>>> GetPageUsersWithRolesAsync(PaginationRequest request, string roleName)
        {
            var resultBuilder = OperationResult<PaginationResponse<UserRolesDto>>.CreateBuilder();

            var filtered = _context.UserRoles
                .Include(userRole => userRole.Role)
                .Include(userRole => userRole.User)
                .Where(CreatePredicate(roleName))
                .Select(userRole => userRole.User);

            var page = await filtered
                .GetPage(request.Page, request.PageSize)
                .ToListAsync();

            var dtos = await page.SelectAsync(async user => await MapAsync(user));

            return resultBuilder.SetValue(new PaginationResponse<UserRolesDto>(dtos, request.Page, request.PageSize,
                await filtered.CountAsync()))
                .BuildResult();
        }

        private Expression<Func<ApplicationUserRole, bool>> CreatePredicate(string roleName)
        {
            var predicate = PredicateBuilder.New<ApplicationUserRole>(true);

            if (!string.IsNullOrEmpty(roleName))
            {
                Expression<Func<ApplicationUserRole, bool>> conditions = userRole =>
                    userRole.Role.Name.ToLower() == roleName.ToLower();

                predicate.And(conditions);
            }

            return predicate;
        }

        private async Task<UserRolesDto> MapAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = await _context.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            return new UserRolesDto(user.Email, roles.Select(r => r.Name), userRoles);
        }

        public async Task<IResult<UserRolesDto>> GetUserRolesByEmailAsync(string email)
        {
            var resultBuilder = OperationResult<UserRolesDto>.CreateBuilder();
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return resultBuilder
                    .AppendError($"The user was not found by the specified email: {email}")
                    .BuildResult();
            }

            var dto = await MapAsync(user);

            return resultBuilder.SetValue(dto).BuildResult();
        }

        public async Task<IResult<IEnumerable<RoleStatisticsResponse>>> GetRoleStatisticsAsync()
        {
            var resultBuilder = OperationResult<IEnumerable<RoleStatisticsResponse>>.CreateBuilder();

            var statistics = await _context.Roles
                .Include(r => r.UserRoles)
                .Select( r  => new RoleStatisticsResponse 
                {
                    RoleName = r.Name,
                    UsersCount = r.UserRoles.Count
                })
                .ToListAsync();

            return resultBuilder.SetValue(statistics).BuildResult();
        }

        public async Task<IResult> EditUserRolesAsync(string email, IEnumerable<string> roles)
        {
            var resultBuilder = OperationResult.CreateBuilder();
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return resultBuilder
                    .AppendError($"The user: {email} was not found")
                    .BuildResult();
            }

            var result = await IsRolesExistAsync(roles);

            if (!result.IsSuccess)
            {
                return resultBuilder
                    .AppendErrors(result.Errors)
                    .BuildResult();
            }

            try
            {
                await ChangeRolesAsync(user, roles);
            }
            catch (Exception ex)
            {
                resultBuilder.AppendError(ex.Message);
            }

            return resultBuilder.BuildResult();
        }

        private async Task ChangeRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any(role => role == Roles.Admin))
            {
                throw new NoAccessChangRoleException($"Cannot change the {Roles.Admin} role");
            }

            if (roles.Any(role => role == Roles.Admin))
            {
                throw new NoAccessChangRoleException($"Cannot change the {Roles.Admin} role");
            }

            /// Search for roles that need to be deleted
            var removedRoles = userRoles.Except(roles);
            /// Search for roles to add
            var addedRoles = roles.Except(userRoles);

            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            await _userManager.AddToRolesAsync(user, addedRoles);
        }

        private async Task<IResult> IsRolesExistAsync(IEnumerable<string> roles)
        {
            var resultBuildr = OperationResult.CreateBuilder();

            foreach (var role in roles)
            {
                var anyRole = await _roleManager.FindByNameAsync(role);

                if (anyRole is null)
                {
                    resultBuildr.AppendError($"The specified role: {role} does not exist");
                }
            }

            return resultBuildr.BuildResult();
        }
    }
}
