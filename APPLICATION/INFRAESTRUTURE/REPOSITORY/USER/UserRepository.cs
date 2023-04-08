using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.BASE;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.USER;

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
    /// <param name="userEntity"></param>
    /// <param name="password"></param>
    /// <param name="isPersistent"></param>
    /// <param name="lockoutOnFailure"></param>
    /// <returns></returns>
    public async Task<SignInResult> PasswordSignInAsync(UserEntity userEntity, string password, bool isPersistent, bool lockoutOnFailure)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(PasswordSignInAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Autenticando usuário com métodos do Identity.\n");

        return await _signInManager.PasswordSignInAsync(userEntity, password, isPersistent, lockoutOnFailure);
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

        Log.Information($"[LOG INFORMATION] - Criando usuário com métodos do Identity.\n");

        return await _userManager.CreateAsync(userEntity, password);
    }

    /// <summary>
    /// Método responsavel por atualizar um usuário.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<IdentityResult> UpdateUserAsync(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(UpdateUserAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Atualizando usuário com métodos do Identity.\n");

        return await _userManager.UpdateAsync(userEntity);
    }

    /// <summary>
    /// Método responsável por recuperar um usuário.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserEntity> GetAsync(Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Recuperando usuário Identity.\n");

        var userEntity = await _userManager.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId));

        return userEntity;
    }

    /// <summary>
    /// Método responsável por recuperar um usuário pelo username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<UserEntity> GetWithUsernameAsync(string username)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetWithUsernameAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Recuperando usuário Identity pelo login.\n");

        return await _userManager.Users.FirstOrDefaultAsync(user => user.UserName.Equals(username));
    }

    /// <summary>
    /// Método responsável por recuperar um usuário completo.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserEntity> GetFullAsync(Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetFullAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Recuperando usuário completo Identity.\n");

        return await _userManager.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId));
    }

    /// <summary>
    /// Método responsável por setar o nome de usuário.
    /// </summary>
    /// <param name="userIdentity"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<IdentityResult> SetUserNameAsync(UserEntity userEntity, string username)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(SetUserNameAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Setando nome de usuário.\n");

        return await _userManager.SetUserNameAsync(userEntity, username);
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

        Log.Information($"[LOG INFORMATION] - Mudando senha de usuário.\n");

        return await _userManager.ChangePasswordAsync(userEntity, currentPassword, password);
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

        Log.Information($"[LOG INFORMATION] - Mudando e-mail de usuário.\n");

        return await _userManager.SetEmailAsync(userEntity, email);
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

        Log.Information($"[LOG INFORMATION] - Mudando celular de usuário.\n");

        return await _userManager.SetPhoneNumberAsync(userEntity, phoneNumber);
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

        Log.Information($"[LOG INFORMATION] - Confirmando e-mail de usuário.\n");

        return await _userManager.ConfirmEmailAsync(userEntity, code);
    }

    /// <summary>
    /// Método responsável por confirmar um usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <returns></returns>
    public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GenerateEmailConfirmationTokenAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Gerando código de confirmação de e-mail de usuário.\n");

        return await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
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

        Log.Information($"[LOG INFORMATION] - Adicionando uma claim em um usuário.\n");

        return await _userManager.AddClaimAsync(userEntity, claim);
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

        Log.Information($"[LOG INFORMATION] - Removendo uma claim em um usuário.\n");

        return await _userManager.RemoveClaimAsync(userEntity, claim);
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

        Log.Information($"[LOG INFORMATION] - Adicionando uma role em um usuário.\n");

        return await _userManager.AddToRoleAsync(userEntity, roleName);
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

        Log.Information($"[LOG INFORMATION] - Removendo uma role em um usuário.\n");

        return await _userManager.RemoveFromRoleAsync(userEntity, roleName);
    }

    /// <summary>
    /// Método responsável por recuperar as roles de usuário.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <returns></returns>
    public async Task<IList<string>> GetUserRolesAsync(UserEntity userEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetUserRolesAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Recuperando as roles de usuário.\n");

        return await _userManager.GetRolesAsync(userEntity);
    }

    /// <summary>
    /// Método responsável por recuperar uma role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<RoleEntity> GetRoleAsync(string roleName)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetRoleAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Recuperando uma role.\n");

        return await _roleManager.Roles.FirstOrDefaultAsync(role => role.Name.Equals(roleName));
    }

    /// <summary>
    /// Método responsável por recuperar as claims de uma role.
    /// </summary>
    /// <param name="roleEntity"></param>
    /// <returns></returns>
    public async Task<IList<Claim>> GetRoleClaimsAsync(RoleEntity roleEntity)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(GetRoleClaimsAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Recuperando as claims uma role.\n");

        return await _roleManager.GetClaimsAsync(roleEntity);
    }

    /// <summary>
    /// Método responsável por recuperar as claims de uma role.
    /// </summary>
    /// <param name="roleEntity"></param>
    /// <returns></returns>
    public async Task SetUserAuthenticationTokenAsync(UserEntity userEntity, string providerName, string tokenName, string token)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UserRepository)} - METHOD {nameof(SetUserAuthenticationTokenAsync)}\n");

        Log.Information($"[LOG INFORMATION] - Setando token gerado ao usuário.\n");

        await _userManager.RemoveAuthenticationTokenAsync(userEntity, providerName, tokenName);

        await _userManager.SetAuthenticationTokenAsync(userEntity, providerName, tokenName, token);

        Log.Information($"[LOG INFORMATION] - Token {token} setado com sucesso ao usuário {userEntity.Email}.\n");
    }
}
