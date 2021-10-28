using Ember.Application.Interfaces.Services;
using Ember.Domain.Contracts;
using Ember.Infrastructure.Data.Entitys;
using Ember.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _userRolesService;

        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet]
        public async Task<ActionResult<IResult<IEnumerable<UserRolesDto>>>> GetAll([FromQuery] PaginationRequest pagination,
            string role = "")
        {
            return Ok(await _userRolesService.GetPageUsersWithRolesAsync(pagination, role));
        }

        [HttpGet("GetByEmail")]
        public async Task<ActionResult<IResult<UserRolesDto>>> GetByEmail([FromQuery] string email)
        {
            return Ok(await _userRolesService.GetUserRolesByEmailAsync(email));
        }

        [HttpGet("GetRoleStatistics")]
        public async Task<ActionResult<IResult<IEnumerable<RoleStatisticsResponse>>>> GetRoleStatistics()
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