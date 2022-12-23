using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IRoleService
{
    /// <summary>
    /// Método responsável por criar uma nova role.
    /// </summary>
    /// <param name="roleRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> CreateAsync(RoleRequest roleRequest);

    /// <summary>
    /// Método responsável por adicionar uma nova lista de claims na role.
    /// </summary>
    /// <param name="roleRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddClaimsAsync(RoleRequest roleRequest);

    /// <summary>
    /// Método responsavel por remover uma lista de claims na role.
    /// </summary>
    /// <param name="roleRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> RemoveClaimsAsync(RoleRequest roleRequest);
}
