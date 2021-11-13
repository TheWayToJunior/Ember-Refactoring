using Ember.Application.Interfaces.Services;
using Ember.Domain.Contracts;
using Ember.Shared;
using Ember.Shared.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Admin)]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _userRolesService;

        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet]
        public async Task<ActionResult<IResult<PaginationResponse<UserRolesDTO>>>> GetAll([FromQuery] PaginationRequest pagination,
            string role)
        {
            return Ok(await _userRolesService.GetPageUsersWithRolesAsync(pagination, role));
        }

        [HttpGet("GetByEmail")]
        public async Task<ActionResult<IResult<UserRolesDTO>>> GetByEmail([FromQuery] string email)
        {
            return Ok(await _userRolesService.GetUserRolesByEmailAsync(email));
        }

        [HttpGet("GetRoleStatistics")]
        public async Task<ActionResult<IResult<IEnumerable<RoleStatistics>>>> GetRoleStatistics()
        {
            return Ok(await _userRolesService.GetRoleStatisticsAsync());
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<IResult>> Edit(string email, [FromBody] IEnumerable<string> roles)
        {
            return Ok(await _userRolesService.EditUserRolesAsync(email, roles));
        }
    }
}