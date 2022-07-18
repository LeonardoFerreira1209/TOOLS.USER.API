using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;

/// <summary>
/// Classe responsável por envio de e-mails.
/// </summary>
public class EmailFacade
{
    private readonly IOptions<AppSettings> _appsettings;

    private readonly IEmailExternal _emailExternal;

    public EmailFacade(IOptions<AppSettings> appsettings, IEmailExternal emailExternal)
    {
        _appsettings = appsettings;

        _emailExternal = emailExternal;
    }

    /// <summary>
    /// Metodo que prepara o envio do e-mail.
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="Exception"></exception>
    public async Task Invite(MailRequest request)
    {
        try
        {
            var response = await _emailExternal.Invite(request);

            if (response.IsSuccessStatusCode is not true) throw new Exception("Erro ao enviar e-mail de confirmação para o usuário. Entre em contato com o suporte e abra um ticket.");
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
}
