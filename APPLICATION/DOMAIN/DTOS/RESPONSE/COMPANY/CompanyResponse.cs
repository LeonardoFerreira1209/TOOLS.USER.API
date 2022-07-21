namespace APPLICATION.DOMAIN.DTOS.RESPONSE.COMPANY;

public class CompanyResponse
{
    #region Base
    /// <summary>
    /// Identificador.
    /// </summary>
    public Guid Id { get; set; }

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
    public DateOnly StartDate { get; set; }
    #endregion
}
