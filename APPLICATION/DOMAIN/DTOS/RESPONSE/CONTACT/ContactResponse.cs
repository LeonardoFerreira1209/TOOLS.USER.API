using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;

public class ContactResponse
{
    /// <summary>
    /// Identificador
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    #region Address
    /// <summary>
    /// Codigo postal do endereço.
    /// </summary>
    public string CEP { get; set; }

    /// <summary>
    /// Número do endereço.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Complemento do endereço.
    /// </summary>
    public string Complement { get; set; }
    #endregion

    #region Email
    /// <summary>
    /// Email de contato.
    /// </summary>
    public string Email { get; set; }
    #endregion

    #region PhoneNumber
    /// <summary>
    /// Numero do celular de contato.
    /// </summary>
    public string PhoneNumber { get; set; }
    #endregion

    #region Person
    /// <summary>
    /// Identificador de pessoa
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Dados da pessoa.
    /// </summary>
    public PersonResponse Person { get; set; }
    #endregion
}
