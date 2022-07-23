using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER;

/// <summary>
/// Request de dados de ativação do usuário.
/// </summary>
public class ActivateUserRequest
{

    public ActivateUserRequest(string code, Guid userId)
    {
        Code = code;

        UserId = userId;
    }

    /// <summary>
    /// Código de confirmação do e-mail.
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Id do usuário que será ativado.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
}
