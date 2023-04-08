namespace APPLICATION.DOMAIN.DTOS.EMAIL;

/// <summary>
/// Classe de recebimento de parametos para o envio do e-mail.
/// </summary>
public class MailRequest
{
    /// <summary>
    /// Lista de remetentes.
    /// </summary>
    public List<string> Receivers { get; set; }

    /// <summary>
    /// Assunto do e-mail
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Nome do template
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Conteudo do e-mail (texto).
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// link do botão
    /// </summary>
    public string Link { get; set; } = "#";

    /// <summary>
    /// texto do botão
    /// </summary>
    public string ButtonText { get; set; }
}
