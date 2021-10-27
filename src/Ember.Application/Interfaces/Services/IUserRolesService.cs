using Ember.Shared;
using Ember.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces.Services
{
    public interface IUserRolesService
    {
        Task<IResult<PaginationResponse<UserRolesDto>>> GetPageUsersWithRolesAsync(PaginationRequest request, string roleFilter);

        Task<IResult<UserRolesDto>> GetUserRolesByEmailAsync(string email);

        Task<IResult<IEnumerable<RoleStatisticsResponse>>> GetRoleStatisticsAsync();

        Task<IResult> EditUserRolesAsync(string email, IEnumerable<string> roles);
    }
}
