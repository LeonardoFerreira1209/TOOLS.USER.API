using APPLICATION.DOMAIN.ENTITY.PLAN;
using APPLICATION.ENUMS;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.ENTITY.USER;

public class UserEntity : IdentityUser<Guid>
{
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
    /// Plano vinculado ao usuário.
    /// </summary>
    public Guid? PlanId { get; set; }

    /// <summary>
    /// Dados do Plano
    /// </summary>
    public virtual PlanEntity Plan { get; set; }

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
