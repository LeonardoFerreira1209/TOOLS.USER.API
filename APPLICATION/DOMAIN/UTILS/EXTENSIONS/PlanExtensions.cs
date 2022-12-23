using APPLICATION.DOMAIN.DTOS.RESPONSE.PLAN;
using APPLICATION.DOMAIN.ENTITY.PLAN;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class PlanExtensions
{
    /// <summary>
    /// Convert plan to response.
    /// </summary>
    /// <param name="plan"></param>
    /// <returns></returns>
    public static PlanResponse ToResponse(this PlanEntity plan)
    {
        return new PlanResponse
        {
            CreatedUserId = plan.CreatedUserId,
            PlanCost = plan.PlanCost,
            PlanDescription = plan.PlanDescription,
            PlanName = plan.PlanName,
            RoleId = plan.RoleId,
            Role = plan.Role?.ToResponse(),
            Created = plan.Created,
            Status = plan.Status,
            TotalMonthsPlan = plan.TotalMonthsPlan,
            Updated = plan.Updated,
            UpdatedUserId = plan.UpdatedUserId
        };
    }
 
}