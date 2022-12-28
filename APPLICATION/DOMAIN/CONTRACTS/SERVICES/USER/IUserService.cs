using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using Microsoft.AspNetCore.Http;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IUserService
{
    /// <summary>
    /// Método responsável por fazer a autenticação do usuário
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AuthenticationAsync(LoginRequest loginRequest);

    /// <summary>
    /// Recuperar usuário através do Id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> GetFullAsync(Guid userId);

    /// <summary>
    /// Método responsável por criar um novo usuário.
    /// </summary>
    /// <param name="userCreateRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> CreateAsync(UserCreateRequest userCreateRequest);

    /// <summary>
    /// Método responsável por atualizar um usuário.
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> UpdateAsync(UserUpdateRequest userUpdateRequest);

    /// <summary>
    /// Método responsável por atualizar a imagem de um usuario.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="formFile"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> UpdateUserIamgeAsync(Guid id, IFormFile formFile);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="activateUserRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> ActivateAsync(ActivateUserRequest activateUserRequest);

    /// <summary>
    /// Método responsavel por adicionar uma claim ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddClaimAsync(string username, ClaimRequest claimRequest);

    /// <summary>
    /// Método responsavel por remover uma claim do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveClaimAsync(string username, ClaimRequest claimRequest);

    /// <summary>
    /// Método responsavel por adicionar uma role ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddRoleAsync(string username, string roleName);

    /// <summary>
    /// Método responsavel por recuperar roles do usuário.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> GetUserRolesAsync(Guid userId);

    /// <summary>
    /// Método responsavel por remover uma role do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveRoleAsync(string username, string roleName);
}
