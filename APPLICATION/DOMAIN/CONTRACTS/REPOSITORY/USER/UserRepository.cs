using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.BASE;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.USER;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.USER;

[ExcludeFromCodeCoverage]
public class UserRepository : BaseRepository, IUserRepository
{
    private readonly SignInManager<UserEntity> _signInManager;

    private readonly UserManager<UserEntity> _userManager;

    private readonly RoleManager<RoleEntity> _roleManager;

    public UserRepository(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, IOptions<AppSettings> appssetings) : base(appssetings)
    {
        _signInManager = signInManager;

        _userManager = userManager;

        _roleManager = roleManager;
    }

    /// <summary>
    /// Retorna o resultado de autenicação do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="isPersistent"></param>
    /// <param name="lockoutOnFailure"></param>
    /// <returns></returns>
    public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(PasswordSignInAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Autenticando usuário com métodos do Identity.\n");

            // sigin user wirh username & password.
            var signInResult = await _signInManager.PasswordSignInAsync(username, password, isPersistent, lockoutOnFailure);

            // return signInResult.
            return signInResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsavel por criar um novo usuário.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<IdentityResult> CreateUserAsync(UserEntity userEntity, string password)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(CreateUserAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Criando usuário com métodos do Identity.\n");

            // created user.
            var identityResult = await _userManager.CreateAsync(userEntity, password);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsavel por atualizar um usuário.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<IdentityResult> UpdateUserAsync(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(UpdateUserAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Atualizando usuário com métodos do Identity.\n");

            // uodated user.
            var identityResult = await _userManager.UpdateAsync(userEntity);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por recuperar um usuário.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserEntity> GetAsync(Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando usuário Identity.\n");

            var userEntity = await _userManager.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId));

            // return userEntity.
            return userEntity;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por recuperar um usuário pelo username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<UserEntity> GetWithUsernameAsync(string username)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetWithUsernameAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando usuário Identity pelo username.\n");

            var userEntity = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName.Equals(username));

            // return userEntity.
            return userEntity;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por recuperar um usuário completo.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserEntity> GetFullAsync(Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetFullAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando usuário completo Identity.\n");

            var userEntity = await _userManager.Users
                     // Include plan.
                     .Include(user => user.Plan)
                     // Include Plan in user.
                     .ThenInclude(plan => plan.Role)
                     // Split includes and select the first user by Id.s
                     .AsSplitQuery().FirstOrDefaultAsync(user => user.Id.Equals(userId));

            // return userEntity.
            return userEntity;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por setar o nome de usuário.
    /// </summary>
    /// <param name="userIdentity"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<IdentityResult> SetUserNameAsync(UserEntity userIdentity, string username)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(SetUserNameAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Setando nome de usuário.\n");

            var identityResult = await _userManager.SetUserNameAsync(userIdentity, username);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por mudar a senha do usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="currentPassword"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<IdentityResult> ChangePasswordAsync(UserEntity userEntity, string currentPassword, string password)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(ChangePasswordAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Mudando senha de usuário.\n");

            var identityResult = await _userManager.ChangePasswordAsync(userEntity, currentPassword, password);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por setar o e-mail do usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<IdentityResult> SetEmailAsync(UserEntity userEntity, string email)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(SetEmailAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Mudando e-mail de usuário.\n");

            var identityResult = await _userManager.SetEmailAsync(userEntity, email);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    ///  Método responsável por setar o celular do usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public async Task<IdentityResult> SetPhoneNumberAsync(UserEntity userEntity, string phoneNumber)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(SetPhoneNumberAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Mudando celular de usuário.\n");

            var identityResult = await _userManager.SetPhoneNumberAsync(userEntity, phoneNumber);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por confirmar um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<IdentityResult> ConfirmEmailAsync(UserEntity userEntity, string code)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(ConfirmEmailAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Confirmando e-mail de usuário.\n");

            var identityResult = await _userManager.ConfirmEmailAsync(userEntity, code);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por confirmar um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <returns></returns>
    public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GenerateEmailConfirmationTokenAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Gerando código de confirmação de e-mail de usuário.\n");

            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);

            // return confirmationCode.
            return confirmationCode;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por adicionar uma claim em um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="claim"></param>
    /// <returns></returns>
    public async Task<IdentityResult> AddClaimUserAsync(UserEntity userEntity, Claim claim)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(AddClaimUserAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando uma claim em um usuário.\n");

            var identityResult = await _userManager.AddClaimAsync(userEntity, claim);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por remover uma claim em um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="claim"></param>
    /// <returns></returns>
    public async Task<IdentityResult> RemoveClaimUserAsync(UserEntity userEntity, Claim claim)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(RemoveClaimUserAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Removendo uma claim em um usuário.\n");

            var identityResult = await _userManager.RemoveClaimAsync(userEntity, claim);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por adicionar uma role em um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<IdentityResult> AddToUserRoleAsync(UserEntity userEntity, string roleName)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(AddToUserRoleAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando uma role em um usuário.\n");

            var identityResult = await _userManager.AddToRoleAsync(userEntity, roleName);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por remover uma role em um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<IdentityResult> RemoveToUserRoleAsync(UserEntity userEntity, string roleName)
    {

        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(RemoveToUserRoleAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Removendo uma role em um usuário.\n");

            var identityResult = await _userManager.RemoveFromRoleAsync(userEntity, roleName);

            // return identityResult.
            return identityResult;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por recuperar as roles de usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <returns></returns>
    public async Task<IList<string>> GetUserRolesAsync(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetUserRolesAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando as roles de usuário.\n");

            var userRoles = await _userManager.GetRolesAsync(userEntity);

            // return userRoles.
            return userRoles;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por recuperar uma role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<RoleEntity> GetRoleAsync(string roleName)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetRoleAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando uma role.\n");

            var userRole = await _roleManager.Roles.FirstOrDefaultAsync(role => role.Name.Equals(roleName));

            // return userRoles.
            return userRole;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }

    /// <summary>
    /// Método responsável por recuperar as claims de uma role.
    /// </summary>
    /// <param name="roleEntity"></param>
    /// <returns></returns>
    public async Task<IList<Claim>> GetRoleClaimsAsync(RoleEntity roleEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetRoleClaimsAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando as claims uma role.\n");

            var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);

            // return roleClaims.
            return roleClaims;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }
}
