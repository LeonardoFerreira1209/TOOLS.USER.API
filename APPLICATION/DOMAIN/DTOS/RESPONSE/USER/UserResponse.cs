using APPLICATION.DOMAIN.DTOS.RESPONSE.COMPANY;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.USER;

public class UserResponse
{
    /// <summary>
    /// Id do usuário.
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
    /// Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Numero de telefone.
    /// </summary>
    public string PhoneNumber { get; set; }

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
    /// Imagem de perfil.
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
    /// Id da empresa.
    /// </summary>
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// Empresa em que o usuário é vinculado.
    /// </summary>
    public virtual CompanyResponse Company { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Data de atualização
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Usuário de cadastro.
    /// </summary>
    public Guid CreatedUserId { get; set; }

    /// <summary>
    /// Usuário que atualizou.
    /// </summary>
    public Guid? UpdatedUserId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public Status Status { get; set; }
}
