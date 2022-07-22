using APPLICATION.DOMAIN.DTOS.RESPONSE.PROFESSION;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;

namespace APPLICATION.DOMAIN.UTILS.Extensions;

public static class ProfessionExtensions
{
    public static ProfessionResponse ToResponse(this Profession profession)
    {
        return new ProfessionResponse
        {
            PersonId = profession.PersonId,
            CompanyId = profession.CompanyId,
            Description = profession.Description,

            StartDate = profession.StartDate.ToDateOnly(),
            EndDate = profession.EndDate.ToDateOnly(),
            Workload = profession.Workload.ToTimeOnly(),

            Office = profession.Office,
            Wage = profession.Wage,
        };
    }
}
