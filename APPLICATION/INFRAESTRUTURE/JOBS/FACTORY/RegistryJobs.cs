using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES;
using FluentScheduler;
using Serilog;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY
{
    public class RegistryJobs : Registry, IRegistryJobs
    {
        public RegistryJobs()
        {
            try
            {
                NonReentrantAsDefault(); Schedule<IProcessDeleteUserWithoutPersonJob>().ToRunEvery(2400).Seconds();
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERRO] - Falha na inicialização do Job. ({exception.Message})\n");
            }
        }
    }
}
