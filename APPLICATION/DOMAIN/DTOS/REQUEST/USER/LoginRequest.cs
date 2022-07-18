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
    public string Password { get; set; }
}
