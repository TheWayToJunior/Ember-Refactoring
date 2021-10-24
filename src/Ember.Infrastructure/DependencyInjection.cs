using Ember.Application.Interfaces;
using Ember.Application.Interfaces.Data;
using Ember.Infrastructure.Data;
using Ember.Infrastructure.Data.Entitys;
using Ember.Infrastructure.Data.Repositories;
using Ember.Infrastructure.Factories;
using Ember.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Ember.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddScoped<IEmailService, EmailService>();
            collection.AddScoped<IAccountService, AccountService>();
            collection.AddScoped<ITokenFactory, JwtFactory>();
            collection.AddScoped<IAuthenticationServices, AuthenticationServices>();
            collection.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            collection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            collection.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            collection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                });

            return collection;
        }
    }
}
