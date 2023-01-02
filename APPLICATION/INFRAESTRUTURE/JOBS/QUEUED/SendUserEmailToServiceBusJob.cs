using APPLICATION.DOMAIN.DTOS.CONFIGURATION.SERVICEBUS.MESSAGE;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;
using Hangfire;
using Serilog;
using Serilog.Context;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.JOBS.RECURRENT;

[ExcludeFromCodeCoverage]
public class SendUserEmailToServiceBusJob
{
    protected SendUserEmailToServiceBusJob() { }

    /// <summary>
    /// Executa o Job.
    /// </summary>
    /// <param name="userEmailMessageDto"></param>
    public static void Execute(UserEmailMessageDto userEmailMessageDto)
    {
        ProcessSendUserEmailToServiceBusJob(userEmailMessageDto).Wait();
    }

    /// <summary>
    /// Processa o envio dos dados de e-mail para o service bus.
    /// </summary>
    /// <param name="userEmailMessageDto"></param>
    /// <returns></returns>
    public static async Task ProcessSendUserEmailToServiceBusJob(UserEmailMessageDto userEmailMessageDto)
    {
        using (LogContext.PushProperty("Fluxo", "ProcessSendUserEmailToServiceBusJob"))
        using (LogContext.PushProperty("Job", "SendUserEmailToServiceBusJob"))
        {
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
                Log.Error("[LOG ERRO] - Erro ao processar Job - ProcessSendUserEmailToServiceBusJob - ({0}).\n", exception.Message);
            }
        }

    }
}
