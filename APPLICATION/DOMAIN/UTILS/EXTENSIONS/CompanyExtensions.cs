using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.DTOS.RESPONSE.COMPANY;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class CompanyExtensions
{
    /// <summary>
    /// Convert company to response.
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    public static CompanyResponse ToResponse(this CompanyEntity company) => new()
    {
        Id = company.Id,

        Name = company.Name,

        Description = company.Description,

        StartDate = company.StartDate
    };

    /// <summary>
    /// Converto company to entity.
    /// </summary>
    /// <param name="companyRequest"></param>
    /// <returns></returns>
    public static CompanyEntity ToEntity(this CompanyRequest companyRequest, Guid userId) => new()
    {
        Description = companyRequest.Description,

        Name = companyRequest.Name,

        StartDate = companyRequest.StartDate,

        Created = DateTime.Now,

        CreatedUserId = userId,

        PlanId = companyRequest.PlanId,

        Status = Status.Active,
    };
}