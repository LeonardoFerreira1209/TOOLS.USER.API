using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES.RECURRENT;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.JOBS.RECURRENT;

[ExcludeFromCodeCoverage]
public class ProcessDeleteUserWithoutPersonJob : IProcessDeleteUserWithoutPersonJob
{
    private readonly bool _execute = false;

    private readonly IOptions<AppSettings> _configuracoes;

    public ProcessDeleteUserWithoutPersonJob(IOptions<AppSettings> configuracoes)
    {
        _configuracoes = configuracoes;
    }

    public void Execute()
    {
        DeleteUserWithoutPerson().Wait();
    }

    public async Task DeleteUserWithoutPerson()
    {
        using (LogContext.PushProperty("Fluxo", "ProcessDeleteUserWithoutPerson"))
        using (LogContext.PushProperty("Job", "ProcessDeleteUserWithoutPersonJob"))
        {
            try
            {
                if (_execute) // Criar config no appssetings para ativar/desativar Job.
                {
                    Log.Information($"[LOG INFORMATION] - Processando Job de Delete de usuários sem pessaos vinculadas.\n");

                    //await new UserRepository(_configuracoes).DeleteUserWithoutPerson();

                    Log.Information("[LOG INFORMATION] - Finalizando Job de Delete de usuários sem pessaos vinculadas\n");
                }
                else
                {
                    Log.Warning("[LOG WARNING] - Toggle para processar Job de Delete de usuários sem pessaos vinculadas desativada - {0}.\n", _execute);
                }
            }
            catch (Exception exception)
            {
                Log.Error("[LOG ERRO] - Erro ao processar Job - ProcessFailedPersonCreateJob - ({0}).\n", exception.Message);
            }
        }

    }
}
