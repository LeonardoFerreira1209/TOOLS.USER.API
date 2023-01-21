using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using Microsoft.AspNetCore.Http;

namespace APPLICATION.DOMAIN.CONTRACTS.FACADE;

/// <summary>
/// Interface do Facade de Utils.
/// </summary>
public interface IUtilFacade
{
    /// <summary>
    /// Método responsável por enviar arquivo para o Facade do TOOLS.UTIL.API.
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> SendAsync(IFormFile formFile);
}
