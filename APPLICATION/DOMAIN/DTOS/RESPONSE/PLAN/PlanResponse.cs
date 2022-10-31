using APPLICATION.DOMAIN.DTOS.RESPONSE.USER.ROLE;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.PLAN;

public class PlanResponse
{
    /// <summary>
    /// Nome do plano
    /// </summary>
    public string PlanName { get; set; }

    /// <summary>
    /// Descrição do plano
    /// </summary>
    public string PlanDescription { get; set; }

    /// <summary>
    /// Valor do plano
    /// </summary>
    public double PlanCost { get; set; }

    /// <summary>
    /// Permissões do plano
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Role
    /// </summary>
    public virtual RolesResponse Role { get; set; }

    /// <summary>
    /// Total de meses do plano
    /// </summary>
    public int TotalMonthsPlan { get; set; }

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
