using APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PROFESSION;
using APPLICATION.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;

public class PersonResponse
{
    #region Base
    /// <summary>
    /// Identificador.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Primeiro nome.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Ultimo nome.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Idade.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Data de aniversário.
    /// </summary>
    public DateOnly BirthDay { get; set; }

    /// <summary>
    /// Gênero.
    /// </summary>
    public Gender Gender { get; set; }
    #endregion

    #region Docs
    /// <summary>
    /// RG.
    /// </summary>
    public string RG { get; set; }

    /// <summary>
    /// CPF.
    /// </summary>
    public string CPF { get; set; }
    #endregion

    #region User
    /// <summary>
    /// Id do usuário.
    /// </summary>
    public Guid UserId { get; set; }
    #endregion

    #region Profession
    /// <summary>
    /// Profissões vinculadas a pessoa.
    /// </summary>
    public ICollection<ProfessionResponse> Professions { get; set; }
    #endregion

    #region Contact
    public ICollection<ContactResponse> Contacts { get; set; }
    #endregion
}
