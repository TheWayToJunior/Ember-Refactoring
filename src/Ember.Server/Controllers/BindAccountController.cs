using Ember.Server.Exceptions;
using Ember.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BindAccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBindAccountService service;

        public BindAccountController(UserManager<IdentityUser> userManager, IBindAccountService userAccount)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.service = userAccount ?? throw new ArgumentNullException(nameof(userAccount));
        }

        [HttpGet]
        public async Task<ActionResult<Account>> GetAccount([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email)
                .ConfigureAwait(true);

            if (user == null)
            {
                return NotFound($"The user \"{email}\" was not found");
            }

            try
            {
                return await service.GetAccount(user.Id)
                    .ConfigureAwait(true);
            }
            catch (Exception)
            {
                /// Значит никаких привязок нет
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Bind([FromBody] BindAccountInfo info)
        {
            if(info == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByEmailAsync(info.Email)
                .ConfigureAwait(true);

            if (user == null)
            {
                return NotFound($"The user \"{info.Email}\" was not found");
            }

            /// Эта учетная запись уже может быть связана
            var isBonded = await service.CheckBinding(user)
                .ConfigureAwait(true);

            if (isBonded)
            {
                return BadRequest("This user already has a binding");
            }

            try
            {
                await service.Bind(user, info.Number)
                    .ConfigureAwait(true);

                return Ok("The personal account was linked successfully");
            }
            catch (NoSpecifiedElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Unlink([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email)
                .ConfigureAwait(true);

            if (user == null)
            {
                return NotFound($"The user \"{email}\" was not found");
            }

            var isBonded = await service.CheckBinding(user)
                .ConfigureAwait(true);

            if (!isBonded)
            {
                return BadRequest("This user does not have a binding");
            }

            try
            {
                await service.Unlink(user)
                    .ConfigureAwait(true);

                return Ok("The personal account was unlinked successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
