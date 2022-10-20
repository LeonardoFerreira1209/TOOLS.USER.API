using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.COMPANY;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.FILE;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.FILE;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.DOMAIN.VALIDATORS;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.COMPANY;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;
using APPLICATION.INFRAESTRUTURE.SIGNALR.CLIENTS;
using APPLICATION.INFRAESTRUTURE.SIGNALR.HUBS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace APPLICATION.APPLICATION.SERVICES.COMPANY;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    /// <summary>
    /// Método responsavel por criar uma empresa.
    /// </summary>
    /// <param name="companyRequest"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> Create(CompanyRequest companyRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(CompanyService)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Criando empresa referente ao usuário.\n");

            // Create company
            var (success, company) = await _companyRepository.Create(companyRequest, userId);

            // Is success...
            if (success is true)
            {
                Log.Information($"[LOG INFORMATION] - Empresa criada com sucesso.\n");

                // Response success.
                return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.SuccessCreated, company.ToResponse(), new List<DadosNotificacao> { new DadosNotificacao("Empresa criada com sucesso!") });
            }

            // Response error.
            return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao criar empresa!") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
}
