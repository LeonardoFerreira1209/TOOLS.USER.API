using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.DTOS.RESPONSE.COMPANY;
using APPLICATION.DOMAIN.ENTITY.COMPANY;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class CompanyExtensions
{
    /// <summary>
    /// Convert company to response.
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    public static CompanyResponse ToResponse(this Company company) => new()
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
    public static Company ToIdentity(this CompanyRequest companyRequest) => new()
    {
        Id = companyRequest.Id,
        Description = companyRequest.Description,
        Name = companyRequest.Name,
        StartDate = companyRequest.StartDate
    };
}