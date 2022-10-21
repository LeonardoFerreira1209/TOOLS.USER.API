using APPLICATION.DOMAIN.DTOS.RESPONSE.COMPANY;
using APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;

public class PersonResponse
{
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
    public string BirthDay { get; set; }

    /// <summary>
    /// Gênero.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Imagem da Pessoa.
    /// </summary>
    public string ImageUri { get; set; }

    /// <summary>
    /// RG.
    /// </summary>
    public string RG { get; set; }

    /// <summary>
    /// CPF.
    /// </summary>
    public string CPF { get; set; }
    
    /// <summary>
    /// Status.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Id do usuário.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Entidade de usuários.
    /// </summary>
    public UserEntity User { get; set; }

    /// <summary>
    /// Id da empresa.
    /// </summary>
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// Entidade da empresa.
    /// </summary>
    public CompanyResponse Company { get; set; }

    /// <summary>
    /// Coleção de contatos do usuário.
    /// </summary>
    public ICollection<ContactResponse> Contacts { get; set; }
}
