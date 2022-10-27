using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.PLAN;
using APPLICATION.DOMAIN.ENTITY.USER;

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
    /// Vinculo com usuários.
    /// </summary>
    public virtual List<UserEntity> Users { get; set; }
}
