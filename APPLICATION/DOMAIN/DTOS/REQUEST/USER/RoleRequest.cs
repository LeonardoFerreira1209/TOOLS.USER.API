namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER;

/// <summary>
/// Classe de envio de Roles.
/// </summary>
public class RoleRequest
{
    /// <summary>
    /// Nome da role.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Claim que faze parte da role.
    /// </summary>
    public List<ClaimRequest> Claims { get; set; }
}
