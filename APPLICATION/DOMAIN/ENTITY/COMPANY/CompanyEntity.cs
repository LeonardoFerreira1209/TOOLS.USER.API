using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.ENTITY.PLAN;

namespace APPLICATION.DOMAIN.ENTITY.COMPANY;

public class CompanyEntity : BaseEntity
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
    /// Data em que a empresa foi fundada
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Plano vinculado a empresa.
    /// </summary>
    public Guid PlanId { get; set; }

    /// <summary>
    /// Plano
    /// </summary>
    public virtual PlanEntity Plan { get; set; }

    /// <summary>
    /// Vinculo com pessoas.
    /// </summary>
    public virtual List<PersonEntity> Persons { get; set; }
}
