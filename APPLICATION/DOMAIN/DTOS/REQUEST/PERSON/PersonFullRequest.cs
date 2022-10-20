using APPLICATION.DOMAIN.DTOS.REQUEST.CONTACT;
using APPLICATION.ENUMS;
using System.Text.Json.Serialization;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;

public class PersonFullRequest
{
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
    public string BirthDay { get; set; }

    /// <summary>
    /// Gênero.
    /// </summary>
    /// 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }

    /// <summary>
    /// RG.
    /// </summary>
    public string RG { get; set; }

    /// <summary>
    /// CPF.
    /// </summary>
    public string CPF { get; set; }

    /// <summary>
    /// Imagem de perfil.
    /// </summary>
    public string ImageUri { get; set; }

    /// <summary>
    /// Status.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; set; }
    
    /// <summary>
    /// Id do usuário.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Id da empresa.
    /// </summary>
    public Guid? CompanyId { get; set; }

    #region Contact
    public ICollection<ContactRequest> Contacts { get; set; }
    #endregion
}
