using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.PERSON;

namespace APPLICATION.DOMAIN.ENTITY.PROFESSION;

public class ProfessionEntity: BaseEntity
{
    #region Base
    /// <summary>
    /// Cargo
    /// </summary>
    public string Office { get; set; }

    /// <summary>
    /// Descrição sobre a profição.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data de inicio
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data de término
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// trabalho Atualmente
    /// </summary>
    public bool Current { get => EndDate == null; set { } }

    /// <summary>
    /// Sálario
    /// </summary>
    public decimal Wage { get; set; }

    /// <summary>
    /// Carga horária
    /// </summary>
    public decimal Workload { get; set; }
    #endregion

    #region Person
    /// <summary>
    /// Id da pessoa vinculada a essa profissão
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Dados da pessoa.
    /// </summary>
    public virtual PersonEntity Person { get; set; }
    #endregion

    #region Company
    /// <summary>
    /// Id da compania em que trabalha
    /// </summary>
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// Dados da empresa.
    /// </summary>
    public virtual CompanyEntity Company { get; set; }
    #endregion
}
