using APPLICATION.DOMAIN.DTOS.REQUEST;
using Refit;

namespace APPLICATION.DOMAIN.CONTRACTS.API;

/// <summary>
/// Interface de chamada do TOOLS.MAIL.API com Refit.
/// </summary>
public interface IEmailExternal
{
    [Post("/api/mail/invite")]
    Task<HttpResponseMessage> Invite(MailRequest request);
}
