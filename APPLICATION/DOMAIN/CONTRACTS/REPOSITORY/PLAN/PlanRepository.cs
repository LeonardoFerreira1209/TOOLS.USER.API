using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.BASE;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.ENTITY.PLAN;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PLAN;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.PLAN;

[ExcludeFromCodeCoverage]
public class PlanRepository : BaseRepository, IPlanRepository
{
    private readonly Contexto _contexto;

    public PlanRepository(Contexto contexto, IOptions<AppSettings> appssetings) : base(appssetings) 
    {
        _contexto = contexto;
    }

    /// <summary>
    /// Retorna os dados de um Plano com base no Id.
    /// </summary>
    /// <param name="planId"></param>
    /// <returns></returns>
    public async Task<PlanEntity> GetAsync(Guid planId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PlanRepository)} - METHOD {nameof(GetAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando um Plano com base no Id {planId}.\n");

            // get plan by id with role.
            var planEntity = await _contexto.Plans.Include(plan => plan.Role).FirstOrDefaultAsync(plan => plan.Id.Equals(planId));

            // return planEntity.
            return planEntity;
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun null value.
            return null;
        }
    }
}