namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER;

/// <summary>
/// Request para Fazer o login do usuário.
/// </summary>
public class LoginRequest
{
    public LoginRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }

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
