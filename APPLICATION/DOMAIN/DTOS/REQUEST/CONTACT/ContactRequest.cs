using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.CONTACT;

public class ContactRequest
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
    /// 
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    #endregion

    #region PhoneNumber
    /// <summary>
    /// Numero do celular de contato.
    /// </summary>
    /// 
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    #endregion
}
