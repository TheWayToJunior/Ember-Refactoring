using Ember.Shared;
using Ember.Shared.Responses;
using System.Threading.Tasks;

namespace Ember.Application.Interfaces.Services
{
    public interface IUserRolesService
    {
        Task<IResult<PaginationResponse<UserRolesDto>>> GetPageUsersWithRolesAsync(PaginationRequest request, string roleFilter);
    }
}
