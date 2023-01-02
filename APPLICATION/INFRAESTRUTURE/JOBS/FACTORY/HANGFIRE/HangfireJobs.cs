using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES.BASE;
using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES.RECURRENT;
using Hangfire;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY.HANGFIRE;

[ExcludeFromCodeCoverage]
public class HangfireJobs : IHangfireJobs
{
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireJobs(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    /// <summary>
    /// Registrar Jobs
    /// </summary>
    public void RegistrarJobs()
    {
        try
        {
            Log.Information($"[LOG INFORMATION] - Inicializando os Job do Hangfire.\n");

            _recurringJobManager.AddOrUpdate<IProcessDeleteUserWithoutPersonJob>("processa-delete-user-without-person", job => job.Execute(), Cron.Daily, TimeZoneInfo.Local);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERRO] - Falha na inicialização do Job do Hangfire. ({exception.Message})\n");
        }
    }
}
