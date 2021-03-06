using Ember.Application.Interfaces;
using Ember.Domain.Contracts;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IConfiguration _configuration;
        private readonly ITokenFactory _tokenFactory;

        public AuthenticationServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _configuration = configuration;
            _tokenFactory = tokenFactory;

            TokenExpiration = DateTime.UtcNow.AddDays(1);
        }

        public DateTime TokenExpiration { get; }

        public async Task<IResult<UserToken>> RegistrationAsync(AuthenticationReques reques)
        {
            var resultBuilder = OperationResult<UserToken>.CreateBuilder();

            var user = new ApplicationUser(reques.Email, userName: reques.Email);
            var result = await _userManager.CreateAsync(user, reques.Password);

            await _userManager.AddToRolesAsync(user, new[]
            { 
                Roles.User,
                Roles.Consumer
            });

            if (!result.Succeeded)
            {
                return resultBuilder.AppendError("Invalid login attempt")
                    .BuildResult();
            }

            var token = await CreateUserTokenAsync(reques.Email);

            return resultBuilder.SetValue(token)
                .BuildResult();
        }

        public async Task<IResult<UserToken>> LoginAsync(AuthenticationReques reques)
        {
            var resultBuilder = OperationResult<UserToken>.CreateBuilder();

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

        private async Task<UserToken> CreateUserTokenAsync(string email)
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
