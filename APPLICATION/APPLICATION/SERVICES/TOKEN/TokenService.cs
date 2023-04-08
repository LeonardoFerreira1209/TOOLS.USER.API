using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.UTILS.AUTH.TOKEN;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.TOKEN
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly RoleManager<RoleEntity> _roleManager;

        private readonly IOptions<AppSettings> _appsettings;

        public TokenService(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, IOptions<AppSettings> appsettings)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _appsettings = appsettings;
        }

        /// <summary>
        /// Cria o JWT TOKEN
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<(TokenJWT tokenJWT, List<DadosNotificacao> messages)> CreateJsonWebToken(string username)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(TokenService)} - METHOD {nameof(CreateJsonWebToken)}\n");

            Log.Information($"[LOG INFORMATION] - Recuperando dados do token do usuário\n");

            // Return de user.
            var userEntity = await User(username);

            // Valid user.
            if (userEntity is null) return (null, new List<DadosNotificacao>() { new DadosNotificacao("Usuário não foi encontrado.") });

            // Return user roles.
            var roles = await Roles(userEntity);

            // Return user claims.
            var claims = await Claims(userEntity, roles);

            Log.Information($"[LOG INFORMATION] - Criando o token do usuário.\n");

            // Create de token and return.
            return await Task.FromResult(new TokenJwtBuilder()
               .AddUsername(username)
               .AddSecurityKey(JwtSecurityKey.Create(_appsettings.Value.Auth.SecurityKey))
               .AddSubject("HYPER.IO PROJECTS L.T.D.A")
               .AddIssuer(_appsettings.Value.Auth.ValidIssuer)
               .AddAudience(_appsettings.Value.Auth.ValidAudience)
               .AddExpiry(_appsettings.Value.Auth.ExpiresIn)
               .AddRoles(roles.ToList())
               .AddClaims(claims.ToList())
               .Builder(userEntity));
        }

        /// <summary>
        /// Return de User.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<UserEntity> User(string username) => await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));

        /// <summary>
        /// Return de Roles.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<IList<Claim>> Roles(UserEntity user)
        {
            // Return roles.
            var roles = await _userManager.GetRolesAsync(user);

            // Return IList of Claims role type.
            return roles.AsParallel().Select(roles => new Claim("role", roles)).ToList();
        }

        /// <summary>
        /// Return de Claims.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<Claim>> Claims(UserEntity user, IList<Claim> roles)
        {
            // Instance Claim list.
            var claims = new List<Claim>();

            // Return all user claims.
            claims.AddRange(await _userManager.GetClaimsAsync(user));

            // Set a list of role names.
            var rolesName = roles.AsParallel().Select(role => role.Value).ToList();

            // Roles not null and have any value.
            if (roles is not null && roles.Any())
            {
                // Select roles id.
                var identityRoles = await _roleManager.Roles.Where(role => rolesName.Contains(role.Name)).ToListAsync();

                // Get role claims and add in claim array.
                identityRoles.AsParallel().ForAll(identityRole => claims.AddRange(_roleManager.GetClaimsAsync(identityRole).Result));
            }

            // Return claims.
            return claims;
        }
    }
}
