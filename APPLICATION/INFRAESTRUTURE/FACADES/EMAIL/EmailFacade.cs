using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.CONTRACTS.FACADE;
using APPLICATION.DOMAIN.DTOS.REQUEST;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;

/// <summary>
/// Classe responsável por envio de e-mails.
/// </summary>
[ExcludeFromCodeCoverage]
public class EmailFacade : IEmailFacade
{
    private readonly IEmailExternal _emailExternal;

    public EmailFacade(IEmailExternal emailExternal)
    {
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
            await _emailExternal.Invite(request);

            Log.Information($"[LOG INFORMATION] - E-mail enviado com sucesso.\n");
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERRO] - {exception.Message}.\n", exception);

            throw;
        }
    }
}
