using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.ENTITY.COMPANY;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.COMPANY;

public interface ICompanyRepository
{
    /// <summary>
    /// Método responsavel por criar uma empresa no banco de dados.
    /// </summary>
    /// <param name="companyRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<(bool success, CompanyEntity company)> Create(CompanyRequest companyRequest, Guid userId);
}
