using Ember.Application.Interfaces;
using Ember.Infrastructure.Data.Entitys;
using Ember.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ember.Infrastructure.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IConfiguration _configuration;
        private readonly ITokenFactory _tokenFactory;

        public AuthenticationServices(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration, ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _configuration = configuration;
            _tokenFactory = tokenFactory;

            TokenExpiration = DateTime.UtcNow.AddDays(1);
        }

        public DateTime TokenExpiration { get; }

        public async Task<IResult<UserTokenResponse>> RegistrationAsync(AuthenticationReques reques)
        {
            var resultBuilder = OperationResult<UserTokenResponse>.CreateBuilder();

            var user = new User(reques.Email, userName: reques.Email);
            var result = await _userManager.CreateAsync(user, reques.Password);

            await _userManager.AddToRoleAsync(user, Roles.User);

            if (!result.Succeeded)
            {
                return resultBuilder.AppendError("Invalid login attempt")
                    .BuildResult();
            }

            var token = await CreateUserTokenAsync(reques.Email);

            return resultBuilder.SetValue(token)
                .BuildResult();
        }

        public async Task<IResult<UserTokenResponse>> LoginAsync(AuthenticationReques reques)
        {
            var resultBuilder = OperationResult<UserTokenResponse>.CreateBuilder();

            var signInResult = await _signInManager.PasswordSignInAsync(reques.Email, reques.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                resultBuilder.AppendError("Username or password invalid");
                return resultBuilder.BuildResult();
            }

            var token = await CreateUserTokenAsync(reques.Email);

            return resultBuilder.SetValue(token)
                .BuildResult();
        }

        private async Task<UserTokenResponse> CreateUserTokenAsync(string email)
        {
            var userRoles = await GetUserRolesAaync(email);
            return _tokenFactory
                .CreateToken(userRoles, email, _configuration["Jwt:Key"], TokenExpiration);
        }

        private async Task<IList<string>> GetUserRolesAaync(string email)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            return await _userManager.GetRolesAsync(user);
        }
    }
}
