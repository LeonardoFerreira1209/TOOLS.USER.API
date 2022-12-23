using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.BASE;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.COMPANY;
using Microsoft.Extensions.Options;
using Serilog;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.COMPANY;

public class CompanyRepository : BaseRepository, ICompanyRepository
{
    private readonly Contexto _contexto;

    private readonly IOptions<AppSettings> _appSettings;

    public CompanyRepository(IOptions<AppSettings> appSettings, Contexto contexto) : base(appSettings)
    {
        _contexto = contexto;

        _appSettings = appSettings;
    }

    /// <summary>
    /// Create a Company
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<(bool success, CompanyEntity company)> Create(CompanyRequest companyRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(CompanyRepository)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando empresa no banco de dados.\n");

            // Add company in database.
            var entityEntry = await _contexto.Companies.AddAsync(companyRequest.ToEntity(userId)); await _contexto.SaveChangesAsync();

            // return true value and company.
            return (true, entityEntry.Entity);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun false and a null value.
            return (false, null);
        }
    }
}
