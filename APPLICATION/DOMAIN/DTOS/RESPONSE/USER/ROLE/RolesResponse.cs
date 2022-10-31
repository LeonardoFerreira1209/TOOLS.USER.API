using APPLICATION.ENUMS;
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

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Data de atualização
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Usuário de cadastro.
    /// </summary>
    public Guid CreatedUserId { get; set; }

    /// <summary>
    /// Usuário que atualizou.
    /// </summary>
    public Guid? UpdatedUserId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public Status Status { get; set; }
}
