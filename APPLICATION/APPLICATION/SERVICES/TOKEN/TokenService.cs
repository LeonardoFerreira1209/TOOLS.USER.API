﻿using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.TOKEN
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly IOptions<AppSettings> _appsettings;

        public TokenService(UserManager<IdentityUser<Guid>> userManager, IOptions<AppSettings> appsettings)
        {
            _userManager = userManager;

            _appsettings = appsettings;
        }

        #region Buider
        /// <summary>
        /// Cria o JWT TOKEN
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TokenJWT> CreateJsonWebToken(string username)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(TokenService)} - METHOD {nameof(CreateJsonWebToken)}\n");

            try
            {
                #region Configurations user

                // Return de user.
                var user = await User(username);
                
                // Return user roles.
                var roles = await Roles(user);

                // Return user claims.
                var claims = await Claims(user);
                #endregion

                // Create de token and return.
                return await Task.FromResult(new TokenJwtBuilder()
                   .AddSecurityKey(JwtSecurityKey.Create(_appsettings.Value.Auth.SecurityKey))
                   .AddSubject("HYPER.IO PROJECTS L.T.D.A")
                   .AddIssuer(_appsettings.Value.Auth.ValidIssuer)
                   .AddAudience(_appsettings.Value.Auth.ValidAudience)
                   .AddExpiry(_appsettings.Value.Auth.ExpiresIn)
                   .AddClaims(claims.ToList())
                   .Builder());
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERROR]", exception, exception.Message);

                throw new Exception(exception.Message);
            }
        }
        #endregion

        #region User
        /// <summary>
        /// Return de User.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<IdentityUser<Guid>> User(string username) => await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));
        #endregion

        #region Role
        /// <summary>
        /// Return de Roles.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<IList<string>> Roles(IdentityUser<Guid> user) => await _userManager.GetRolesAsync(user);
        #endregion

        #region Claims
        /// <summary>
        /// Return de Claims.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<Claim>> Claims(IdentityUser<Guid> user) => await Task.FromResult(_userManager.GetClaimsAsync(user).Result.ToList());
        #endregion
    }
}