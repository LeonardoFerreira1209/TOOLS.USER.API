using APPLICATION.DOMAIN.DTOS.REQUEST.CONTACT;
using APPLICATION.DOMAIN.DTOS.REQUEST.PROFESSION;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;

public class PersonFullRequest
{
    #region Base

    /// <summary>
    /// Identificador
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Primeiro nome
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
    public DateTime BirthDay { get; set; }  

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
    public ICollection<ProfessionRequest> Professions { get; set; }
    #endregion

    #region Contact
    public ICollection<ContactRequest> Contacts { get; set; }
    #endregion
}
