using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER;

/// <summary>
/// Request para Fazer o login do usuário.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Senha do usuário
    /// </summary>
    /// 
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
