using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.PLAN;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.ENTITY.PLAN;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.BASE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.PLAN;

[ExcludeFromCodeCoverage]
public class PlanRepository : BaseRepository, IPlanRepository
{
    private readonly Context _context;

    public PlanRepository(Context context, IOptions<AppSettings> appssetings) : base(appssetings)
    {
        _context = context;
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
            var planEntity = await _context.Plans.Include(plan => plan.Role).FirstOrDefaultAsync(plan => plan.Id.Equals(planId));

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