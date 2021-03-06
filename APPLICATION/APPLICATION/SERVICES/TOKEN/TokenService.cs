using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.TOKEN
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IOptions<AppSettings> _appsettings;

        public TokenService(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager, IOptions<AppSettings> appsettings)
        {
            _userManager = userManager;

            _roleManager = roleManager;

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
                Log.Information($"[LOG INFORMATION] - Recuperando dados do token do usuário\n");

                #region Configurations user

                // Return de user.
                var user = await User(username);

                // Return token saved in database.
                var tokenSaved = await TokenSaved(user);

                // verify data in token.
                if (tokenSaved is null)
                {
                    // Return user roles.
                    var roles = await Roles(user);

                    // Return user claims.
                    var claims = await Claims(user, roles);
                    #endregion

                    Log.Information($"[LOG INFORMATION] - Criando o token do usuário.\n");

                    // Create de token and return.
                    var token = await Task.FromResult(new TokenJwtBuilder()
                       .AddUsername(username)
                       .AddSecurityKey(JwtSecurityKey.Create(_appsettings.Value.Auth.SecurityKey))
                       .AddSubject("HYPER.IO PROJECTS L.T.D.A")
                       .AddIssuer(_appsettings.Value.Auth.ValidIssuer)
                       .AddAudience(_appsettings.Value.Auth.ValidAudience)
                       .AddExpiry(_appsettings.Value.Auth.ExpiresIn)
                       .AddRoles(roles.ToList())
                       .AddClaims(claims.ToList())
                       .Builder(user));

                    // Save tokenin database
                    //await _userManager.GenerateUserTokenAsync(user, "TOOLS.USER.API", token.Value);

                    return token;
                }

                return new TokenJWT(new JwtSecurityTokenHandler().ReadJwtToken(tokenSaved));
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

                throw new Exception(exception.Message);
            }
        }
        #endregion

        #region Private methods

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
        private async Task<List<Claim>> Claims(IdentityUser<Guid> user, IList<string> roles)
        {
            var claims = new List<Claim>();

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            if (roles is not null && roles.Any())
            {
                var identityRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

                identityRoles.ForEach(identityRole => claims.AddRange(_roleManager.GetClaimsAsync(identityRole).Result));
            }

            return claims;
        }
        #endregion

        #region Token
        private async Task<string> TokenSaved(IdentityUser<Guid> user) => await _userManager.GetAuthenticationTokenAsync(user, "TOOLS.USER.API", "AUTHTOKEN");
        #endregion

        #endregion
    }
}
