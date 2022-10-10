using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
using APPLICATION.DOMAIN.ENTITY.ROLE;

namespace APPLICATION.DOMAIN.ENTITY.COMPANY;

public class CompanyEntity : BaseEntity
{
    #region Base
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
    public DateTime StartDate { get; set; }
    #endregion

    #region Vinculos
    /// <summary>
    /// Vinculo com profissões
    /// </summary>
    public virtual List<ProfessionEntity> Professions { get; set; }

    /// <summary>
    /// Vinculo com roles.
    /// </summary>
    public virtual List<RoleEntity> Roles { get; set; }
    #endregion
}
