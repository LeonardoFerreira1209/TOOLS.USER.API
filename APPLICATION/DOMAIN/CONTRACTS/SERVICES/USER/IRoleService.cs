using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IRoleService
{
    /// <summary>
    /// Método responsável por criar uma nova role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> Create(RoleRequest request);

    /// <summary>
    /// Método responsável por adicionar uma nova claim na role.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<object>> AddClaim(string roleName, List<ClaimRequest> claimRequests);
}
