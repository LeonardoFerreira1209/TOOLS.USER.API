using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.PERSON;

namespace APPLICATION.DOMAIN.ENTITY.CONTACT;

public class ContactEntity : BaseEntity
{
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
    public string Email { get; set; }
    #endregion

    #region PhoneNumber
    /// <summary>
    /// Numero do celular de contato.
    /// </summary>
    /// 
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
    public virtual PersonEntity Person { get; set; }
    #endregion
}
