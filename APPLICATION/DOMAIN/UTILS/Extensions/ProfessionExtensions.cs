using APPLICATION.DOMAIN.DTOS.REQUEST.PROFESSION;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PROFESSION;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class ProfessionExtensions
{
    public static ProfessionResponse ToResponse(this Profession profession)
    {
        return new ProfessionResponse
        {
            Id = profession.Id,
            PersonId = profession.PersonId,
            CompanyId = profession.CompanyId,
            Description = profession.Description,

            StartDate = profession.StartDate,
            EndDate = profession.EndDate,
            Workload = profession.Workload,

            Office = profession.Office,
            Wage = profession.Wage,

            Company = profession.Company?.ToResponse()
        };
    }

    public static Profession ToIdentity(this ProfessionRequest professionRequest) => new Profession
    {
        PersonId = professionRequest.PersonId,
        CompanyId = professionRequest.CompanyId,
        Description = professionRequest.Description,

        StartDate = professionRequest.StartDate,
        EndDate = professionRequest.EndDate,
        Workload = professionRequest.Workload,

        Office = professionRequest.Office,
        Wage = professionRequest.Wage,
    };
}
