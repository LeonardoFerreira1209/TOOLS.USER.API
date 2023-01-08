using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.PLAN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PLAN;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using Serilog;

namespace APPLICATION.APPLICATION.SERVICES.PLAN;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepository;

    public PlanService(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    /// <summary>
    /// Método responsavel por retornar dados de um plano com base no Id.
    /// </summary>
    /// <param name="planId"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> GetAsync(Guid planId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PlanService)} - METHOD {nameof(GetAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Buscando o plano.\n");

            // Get plan by Id.
            var planEntity = await _planRepository.GetAsync(planId);

            // Is not null.
            if (planEntity is not null)
            {
                Log.Information($"[LOG INFORMATION] - Plano recuperado com sucesso. {planEntity.PlanName}\n");
              
                // Response success.
                return new ApiResponse<object>(true, StatusCodes.SuccessOK, planEntity.ToResponse(), new List<DadosNotificacao> { new DadosNotificacao("Plano recuperado com sucesso.") });
            }

            Log.Information($"[LOG INFORMATION] - Plano não encontrado.\n");

            // Response error.
            return new ApiResponse<object>(false, StatusCodes.ErrorNotFound, null, new List<DadosNotificacao> { new DadosNotificacao("Plano não encontrado.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
}