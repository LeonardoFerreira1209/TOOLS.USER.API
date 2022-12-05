using APPLICATION.DOMAIN.DTOS.RESPONSE.PLAN;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.COMPANY;

public class CompanyResponse
{
    /// <summary>
    /// Identificador.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome da empresa.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Sobre a empresa.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Cnpj da empresa.
    /// </summary>
    public string Cpnj { get; set; }

    /// <summary>
    /// Data em que a empresa foi fundada.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Identificador do plano.
    /// </summary>
    public Guid PlanId { get; set; }

    /// <summary>
    /// Dados plano.
    /// </summary>
    public PlanResponse Plan { get; set; }

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
