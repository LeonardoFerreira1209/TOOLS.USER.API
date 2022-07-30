using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using Microsoft.AspNetCore.Mvc;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IUserService
{
    #region User
    /// <summary>
    /// Método responsável por fazer a autenticação do usuário
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ObjectResult> Authentication(LoginRequest loginRequest);

    /// <summary>
    /// Método responsável por criar um novo usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ObjectResult> Create(PersonFastRequest personFastRequest);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ObjectResult> Activate(ActivateUserRequest activateUserRequest);
    #endregion

    #region Claims
    /// <summary>
    /// Método responsavel por adicionar uma claim ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ObjectResult> AddClaim(string username, ClaimRequest claimRequest);

    /// <summary>
    /// Método responsavel por remover uma claim do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ObjectResult> RemoveClaim(string username, ClaimRequest claimRequest);
    #endregion

    #region Roles
    /// <summary>
    /// Método responsavel por adicionar uma role ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ObjectResult> AddRole(string username, string roleName);

    /// <summary>
    /// Método responsavel por remover uma role do usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ObjectResult> RemoveRole(string username, string roleName);
    #endregion
}
