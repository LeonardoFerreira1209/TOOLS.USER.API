using APPLICATION.DOMAIN.DTOS.REQUEST.PROFESSION;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;

public class CompanyRequest
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
