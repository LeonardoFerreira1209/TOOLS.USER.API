using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER;

/// <summary>
/// Request de dados de ativação do usuário.
/// </summary>
public class ActivateUserRequest
{
    /// <summary>
    /// Código de confirmação do e-mail.
    /// </summary>
    [Required]
    public string Codigo { get; set; }

    /// <summary>
    /// Id do usuário que será ativado.
    /// </summary>
    [Required]
    public Guid UsuarioId { get; set; }
}
