using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.PLAN;

public interface IPlanService
{
    /// <summary>
    /// Método responsável por retornar dados de um Plano
    /// </summary>
    /// <param name="planId"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> GetAsync(Guid planId);
}
