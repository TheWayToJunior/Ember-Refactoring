using Ember.Application.Interfaces;
using Ember.Domain.Contracts;
using Ember.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;

        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<UserToken>> RegistrationAsync([FromBody] AuthenticationReques reques)
        {
            return Ok(await _authenticationServices.RegistrationAsync(reques));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<IResult<UserToken>>> LoginAsync([FromBody] AuthenticationReques reques)
        {
            return Ok(await _authenticationServices.LoginAsync(reques));
        }
    }
}