using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.ENUMS;
using System.Text.Json.Serialization;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;

public class PersonFastRequest
{
    /// <summary>
    /// Primeiro nome
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Ultimo nome
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Sexo
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }
    
    /// <summary>
    /// CPF da pessoa.
    /// </summary>
    public string CPF { get; set; }
    
    /// <summary>
    /// Dados do usuário.
    /// </summary>
    public UserCreateRequest User { get; set; }

    /// <summary>
    /// Dados de cadastro da empresa.
    /// </summary>
    public CompanyRequest Company { get; set; }
}