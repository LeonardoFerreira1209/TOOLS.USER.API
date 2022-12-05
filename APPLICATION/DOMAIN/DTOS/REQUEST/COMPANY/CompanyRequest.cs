namespace APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;

public class CompanyRequest
{
    /// <summary>
    /// Nome da empresa.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Sobre a empresa
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Cnpj da empresa.
    /// </summary>
    public string Cpnj { get; set; }

    /// <summary>
    /// Data em que a empresa foi fundada
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Plano vinculado com a empresa.
    /// </summary>
    public Guid PlanId { get; set; }
}
