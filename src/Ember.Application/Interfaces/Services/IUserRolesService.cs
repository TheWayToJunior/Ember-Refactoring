using Ember.Domain.Contracts;
using Ember.Shared;
using Ember.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces.Services
{
    public interface IUserRolesService
    {
        Task<IResult<PaginationResponse<UserRolesDTO>>> GetPageUsersWithRolesAsync(PaginationRequest request, string roleFilter);

        Task<IResult<UserRolesDTO>> GetUserRolesByEmailAsync(string email);

        Task<IResult<IEnumerable<RoleStatistics>>> GetRoleStatisticsAsync();

        Task<IResult> EditUserRolesAsync(string email, IEnumerable<string> roles);
    }
}
