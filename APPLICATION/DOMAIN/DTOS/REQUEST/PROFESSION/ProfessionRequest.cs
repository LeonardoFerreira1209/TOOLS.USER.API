namespace APPLICATION.DOMAIN.DTOS.REQUEST.PROFESSION;

public class ProfessionRequest
{
    #region Base
    /// <summary>
    /// Idemtificador da profissão.
    /// </summary>
    public Guid Id { get; set; }

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
    public DateTime EndDate { get; set; }

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
    #endregion

    #region Company
    /// <summary>
    /// Id da compania em que trabalha
    /// </summary>
    public Guid CompanyId { get; set; }
    #endregion
}
