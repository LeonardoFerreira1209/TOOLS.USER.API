using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Security.Claims;

namespace APPLICATION.APPLICATION.SERVICES.TOKEN
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IPersonService _personService;

        private readonly IOptions<AppSettings> _appsettings;

        public TokenService(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager, IPersonService personService, IOptions<AppSettings> appsettings)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _personService = personService;

            _appsettings = appsettings;
        }

        #region Buider
        /// <summary>
        /// Cria o JWT TOKEN
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ApiResponse<object>> CreateJsonWebToken(string username)
        {
            Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(TokenService)} - METHOD {nameof(CreateJsonWebToken)}\n");

            try
            {
                Log.Information($"[LOG INFORMATION] - Recuperando dados do token do usuário\n");

                #region Configurations user
                // Return de user.
                var user = await User(username);
                #endregion

                #region Configurations user
                // Return user roles.
                var roles = await Roles(user);

                // Return user claims.
                var claims = await Claims(user, roles);

                // Return personId
                var personId = await PersonId(user.Id);
                #endregion

                Log.Information($"[LOG INFORMATION] - Criando o token do usuário.\n");

                // Create de token and return.
                var response = await Task.FromResult(new TokenJwtBuilder()
                   .AddUsername(username)
                   .AddPersonId(personId)
                   .AddSecurityKey(JwtSecurityKey.Create(_appsettings.Value.Auth.SecurityKey))
                   .AddSubject("HYPER.IO PROJECTS L.T.D.A")
                   .AddIssuer(_appsettings.Value.Auth.ValidIssuer)
                   .AddAudience(_appsettings.Value.Auth.ValidAudience)
                   .AddExpiry(_appsettings.Value.Auth.ExpiresIn)
                   .AddRoles(roles.ToList())
                   .AddClaims(claims.ToList())
                   .Builder(user));

                return response;
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERROR] - {exception.Message}\n");

                return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
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
        private async Task<IList<Claim>> Roles(IdentityUser<Guid> user)
        {
            // Return roles.
            var roles = await _userManager.GetRolesAsync(user);

            // Return IList of Claims role type.
            return roles.Select(roles => new Claim("role", roles)).ToList();
        }
        #endregion

        #region Claims
        /// <summary>
        /// Return de Claims.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<Claim>> Claims(IdentityUser<Guid> user, IList<Claim> roles)
        {
            // Instance Claim list.
            var claims = new List<Claim>();

            // Return all user claims.
            claims.AddRange(await _userManager.GetClaimsAsync(user));

            // Set a list of role names.
            var rolesName = roles.Select(role => role.Value).ToList();

            // Roles not null and have any value.
            if (roles is not null && roles.Any())
            {
                // Select roles id.
                var identityRoles = await _roleManager.Roles.Where(role => rolesName.Contains(role.Name)).ToListAsync();

                // Get role claims and add in claim array.
                identityRoles.ForEach(identityRole => claims.AddRange(_roleManager.GetClaimsAsync(identityRole).Result));
            }

            // Return claims.
            return claims;
        }
        #endregion

        #region Person
        private async Task<Guid> PersonId(Guid userId) => await Task.FromResult(Guid.Parse(_personService.GetIdWithUserId(userId).Result.Dados));
        #endregion

        #endregion
    }
}
