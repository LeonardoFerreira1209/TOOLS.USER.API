using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using Microsoft.Extensions.Options;
using Serilog;

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
        Log.Information($"[LOG INFORMATION] - Enviando e-mail para o usuário.\n");

        try
        {
            var response = await _emailExternal.Invite(request);

            if (response.IsSuccessStatusCode is not true) throw new Exception("Erro ao enviar e-mail de confirmação para o usuário. Entre em contato com o suporte e abra um ticket.");

            Log.Information($"[LOG INFORMATION] - E-mail enviado com sucesso.\n");
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERRO] - {exception.Message}.\n", exception);

            throw new Exception($"Falha ao acessar o serviço de envio de e-mails, {exception.Message}");
        }
    }
}
