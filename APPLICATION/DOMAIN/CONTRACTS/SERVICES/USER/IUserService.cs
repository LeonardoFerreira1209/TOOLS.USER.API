using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IUserService
{
    #region User
    /// <summary>
    /// Método responsável por fazer a autenticação do usuário
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Authentication(LoginRequest request);

    /// <summary>
    /// Método responsável por criar um novo usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Create(PersonRequest request);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Activate(ActivateUserRequest request);
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
