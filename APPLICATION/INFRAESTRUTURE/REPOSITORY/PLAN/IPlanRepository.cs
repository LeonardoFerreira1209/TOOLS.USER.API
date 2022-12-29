using APPLICATION.DOMAIN.ENTITY.PLAN;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.PLAN;

public interface IPlanRepository
{
    /// <summary>
    /// Retorna os dados de um Plano.
    /// </summary>
    /// <param name="planId"></param>
    /// <returns></returns>
    Task<PlanEntity> GetAsync(Guid planId);
}