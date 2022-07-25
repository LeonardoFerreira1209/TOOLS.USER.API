using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using Microsoft.AspNetCore.Mvc;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IRoleService
{
    /// <summary>
    /// Método responsável por criar uma nova role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ObjectResult> Create(RoleRequest request);

    /// <summary>
    /// Método responsável por adicionar uma nova claim na role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ObjectResult> AddClaim(string roleName, List<ClaimRequest> claimRequests);

    /// <summary>
    /// Método responsavel por remover a claim da role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="claimRequests"></param>
    /// <returns></returns>
    Task<ObjectResult> RemoveClaim(string roleName, ClaimRequest claimRequests);
}
