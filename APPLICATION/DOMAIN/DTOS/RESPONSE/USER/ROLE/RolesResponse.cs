using System.Security.Claims;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.USER.ROLE;

public class RolesResponse
{
    /// <summary>
    /// Nome da role
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Claims da role.
    /// </summary>
    public ICollection<Claim> Claims { get; set; }
}
