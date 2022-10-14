using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IRoleService
{
    /// <summary>
    /// Método responsável por criar uma nova role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> Create(RoleRequest roleRequest);

    /// <summary>
    /// Método responsável por adicionar uma nova lista de claims na role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddClaims(RoleRequest roleRequest);

    /// <summary>
    /// Método responsavel por remover uma lista de claims na role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveClaims(RoleRequest roleRequest);
}
