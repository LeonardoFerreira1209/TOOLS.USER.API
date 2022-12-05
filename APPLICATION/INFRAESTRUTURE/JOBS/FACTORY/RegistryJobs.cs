using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES;
using FluentScheduler;
using Hangfire;
using Serilog;

namespace APPLICATION.INFRAESTRUTURE.JOBS.FACTORY
{
    public class RegistryJobs : Registry, IRegistryJobs
    {
        private const string KEY_JOB_PROCESSAR_ANALISE_CREDITO_TESTE = "RedeAceitacaoJobs.Processar_Analise_Credito";
        private const string KEY_JOB_PROCESSAR_EXPURGA_VEICULOS = "RedeAceitacaoJobs.Processar_Expurga_Veiculos";

        public RegistryJobs(IRecurringJobManager recurringJobManager)
        {

            try
            {
                //recurringJobManager.AddOrUpdate<IProcessarAnaliseCreditoJob>("ra-archetype-analise-credito", c => c.ProcessarAnaliseCredito(), GetInterval(KEY_JOB_PROCESSAR_ANALISE_CREDITO_TESTE), TimeZoneInfo.Local);
                //recurringJobManager.AddOrUpdate<IProcessarExpurgaVeiculoJob>("ra-archetype-expurga-veiculo", c => c.ProcessarExpurgacaoVeiculos(), GetInterval(KEY_JOB_PROCESSAR_EXPURGA_VEICULOS), TimeZoneInfo.Local);

                NonReentrantAsDefault(); Schedule<IProcessDeleteUserWithoutPersonJob>().ToRunEvery(2400).Seconds();
            }
            catch (Exception exception)
            {
                Log.Error($"[LOG ERRO] - Falha na inicialização do Job. ({exception.Message})\n");
            }
        }

        private string GetInterval(string key)
        {
            try
            {
                //if (configuracao == null || string.IsNullOrEmpty(configuracao?.Valor))
                    throw new Exception($"Não foi possível encontrar a configuração de sistema {key}");

               // Log.Information($"{key} - Cron de execução: {configuracao.Valor}");

               // return configuracao.Valor;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());

                return "0/5 * * * * *";
            }
        }
    }
}
