using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.COMPANY;

public interface ICompanyService
{
    /// <summary>
    /// Método responsavel por criar uma empresa.
    /// </summary>
    /// <param name="companyRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> Create(CompanyRequest companyRequest, Guid userId);
}
