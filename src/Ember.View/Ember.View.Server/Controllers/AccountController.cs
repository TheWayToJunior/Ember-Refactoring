using Ember.Application.Interfaces;
using Ember.Domain.Contracts;
using Ember.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService userAccount)
        {
            _service = userAccount ?? throw new ArgumentNullException(nameof(userAccount));
        }

        [HttpGet]
        public async Task<ActionResult<IResult<AccountDTO>>> GetAccountAsync([FromQuery] string email)
        {
            return Ok(await _service.GetAccountAsync(email));
        }

        [HttpPost]
        public async Task<ActionResult<IResult>> BindAsync([FromBody] BindingRequest request)
        {
            return Ok(await _service.BindAsync(request.Email, request.Number));
        }

        [HttpDelete]
        public async Task<ActionResult<IResult>> UnlinkAsync([FromQuery] string email)
        {
            return Ok(await _service.UnlinkAsync(email));
        }
    }
}
