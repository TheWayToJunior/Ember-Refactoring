using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> Create([FromBody] UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { Email = userInfo.Email, UserName = userInfo.Email };
            var result = await userManager.CreateAsync(user, userInfo.Password)
                .ConfigureAwait(true);

            await userManager.AddToRoleAsync(user, Roles.User)
                .ConfigureAwait(true);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid login attempt");
            }

            return BuildToken(userInfo, await GetUserRoles(userInfo.Email)
                .ConfigureAwait(true));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,
                isPersistent: false, lockoutOnFailure: false)
                .ConfigureAwait(true);

            if (!result.Succeeded)
            {
                return BadRequest("Username or password invalid");
            }

            return BuildToken(userInfo, await GetUserRoles(userInfo.Email)
                .ConfigureAwait(true));
        }

        private async Task<IList<string>> GetUserRoles(string email)
        {
            var user = await userManager.FindByEmailAsync(email)
               .ConfigureAwait(true);

            return await userManager.GetRolesAsync(user)
                .ConfigureAwait(true);
        }

        private UserToken BuildToken(UserInfo userInfo, IList<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(ClaimTypes.Name, userInfo.Email)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Expiration time
            var expiration = DateTime.UtcNow.AddDays(1);

            JwtSecurityToken jwt = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: cred);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Expiration = expiration
            };
        }
    }
}