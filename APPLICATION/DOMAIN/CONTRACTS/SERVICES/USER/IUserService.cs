using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IUserService
{
    #region User
    /// <summary>
    /// Método responsável por fazer a autenticação do usuário
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Authentication(LoginRequest loginRequest);

    /// <summary>
    /// Método responsável por criar um novo usuário.
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Create(PersonFastRequest personFastRequest);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Activate(ActivateUserRequest activateUserRequest);
    #endregion

    #region Claims
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
    #endregion

    #region Roles
    /// <summary>
    /// Método responsavel por adicionar uma role ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddRole(string username, string roleName);

    /// <summary>
    /// Método responsavel por remover uma role do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveRole(string username, string roleName);
    #endregion
}
