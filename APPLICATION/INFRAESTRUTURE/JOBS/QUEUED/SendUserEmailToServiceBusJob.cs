using APPLICATION.DOMAIN.DTOS.CONFIGURATION.SERVICEBUS.MESSAGE;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;
using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Serilog;
using APPLICATION.INFRAESTRUTURE.JOBS.RECURRENT;

namespace APPLICATION.INFRAESTRUTURE.JOBS.QUEUED;

[ExcludeFromCodeCoverage]
public static class SendUserEmailToServiceBusJob
{
    /// <summary>
    /// Executa o Job.
    /// </summary>
    /// <param name="userEmailMessageDto"></param>
    public static void Execute(UserEmailMessageDto userEmailMessageDto)
    {
        // Execute and wait Job.
        ProcessSendUserEmailToServiceBusJob(userEmailMessageDto).Wait();
    }

    /// <summary>
    /// Processa o envio dos dados de e-mail para o service bus.
    /// </summary>
    /// <param name="userEmailMessageDto"></param>
    /// <returns></returns>
    public static async Task ProcessSendUserEmailToServiceBusJob(UserEmailMessageDto userEmailMessageDto)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(SendUserEmailToServiceBusJob)} - METHOD {nameof(ProcessDeleteUserWithoutPersonJob)}\n");

        try
        {
            Log.Information("[LOG INFORMATION] - Iniciando - Processando Job de envio de dados de e-mail para service bus.\n");

            // Create enqueued job to send message to service bus queue.
            BackgroundJob.Enqueue<IUserEmailServiceBusSenderProvider>(userServiceBusSenderProvider => userServiceBusSenderProvider.SendAsync(userEmailMessageDto, DateTimeOffset.Now));

            // Complete Task.
            await Task.CompletedTask;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERRO] - Erro ao processar Job - {nameof(SendUserEmailToServiceBusJob)} - ({0}).\n", exception.Message);
        }
    }
}
