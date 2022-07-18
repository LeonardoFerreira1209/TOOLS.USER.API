namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER;

/// <summary>
/// Classe de envio de Roles para adicionar no usuario.
/// </summary>
public class RoleToUserRequest
{
    /// <summary>
    /// Lista de nomes das roles.
    /// </summary>
    public List<string> Name { get; set; }
}
