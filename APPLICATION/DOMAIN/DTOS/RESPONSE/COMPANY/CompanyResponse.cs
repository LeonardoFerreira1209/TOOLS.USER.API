using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;

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
    /// Data em que a empresa foi fundada.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Pessoas vinculadas com Empresa.
    /// </summary>
    public ICollection<PersonResponse> Persons { get; set; }

    /// <summary>
    /// Identificador do plano.
    /// </summary>
    public Guid PlanId { get; set; }
}
