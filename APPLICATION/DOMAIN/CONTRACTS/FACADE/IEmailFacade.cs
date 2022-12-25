using APPLICATION.DOMAIN.DTOS.REQUEST;

namespace APPLICATION.DOMAIN.CONTRACTS.FACADE;

public interface IEmailFacade
{
    /// <summary>
    /// Metodo que prepara o envio do e-mail.
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="Exception"></exception>
    Task Invite(MailRequest request);
}
