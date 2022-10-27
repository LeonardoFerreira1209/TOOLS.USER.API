using APPLICATION.DOMAIN.DTOS.REQUEST;
using Refit;

namespace APPLICATION.DOMAIN.CONTRACTS.API;

/// <summary>
/// Interface de chamada do TOOLS.UTIL.API com Refit.
/// </summary>
public interface IEmailExternal
{
    /// <summary>
    /// Enviar e-mail.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Post("/api/mail/invite")]
    Task<HttpResponseMessage> Invite(MailRequest request);
}
