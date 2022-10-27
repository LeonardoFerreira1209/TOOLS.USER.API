using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IUserService
{
    /// <summary>
    /// Método responsável por fazer a autenticação do usuário
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> Authentication(LoginRequest loginRequest);

    /// <summary>
    /// Método responsável por criar um novo usuário.
    /// </summary>
    /// <param name="userCreateRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> Create(UserCreateRequest userCreateRequest);

    /// <summary>
    /// Método responsável por atualizar um usuário.
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> Update(UserUpdateRequest userRequest);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="activateUserRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> Activate(ActivateUserRequest activateUserRequest);

    /// <summary>
    /// Método responsavel por adicionar uma claim ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddClaim(string username, ClaimRequest claimRequest);

    /// <summary>
    /// Método responsavel por remover uma claim do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveClaim(string username, ClaimRequest claimRequest);

    /// <summary>
    /// Método responsavel por adicionar uma role ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddRole(string username, string roleName);

    /// <summary>
    /// Método responsavel por recuperar roles do usuário.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> GetUserRoles(Guid userId);

    /// <summary>
    /// Método responsavel por remover uma role do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveRole(string username, string roleName);
}
