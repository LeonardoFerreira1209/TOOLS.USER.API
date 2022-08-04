using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;

namespace APPLICATION.DOMAIN.ENTITY.COMPANY;

public class Company : BaseEntity
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

    #region Professiond
    public virtual List<Profession> Professions { get; set; }
    #endregion
}
