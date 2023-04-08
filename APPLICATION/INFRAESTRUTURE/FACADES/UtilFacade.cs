using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.CONTRACTS.FACADE;
using APPLICATION.DOMAIN.DTOS.RESPONSE.FILE;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

using StatusCodes = APPLICATION.ENUMS.StatusCodes;

namespace APPLICATION.INFRAESTRUTURE.FACADES;

public class UtilFacade : IUtilFacade
{
    private readonly IExternalUtil _externalUtil;

    public UtilFacade(IExternalUtil externalUtil)  
    { 
        _externalUtil = externalUtil; 
    }

    /// <summary>
    /// Método responsavel por retornar dados de um plano com base no Id.
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> SendAsync(IFormFile formFile)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(UtilFacade)} - METHOD {nameof(SendAsync)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Enviando a imagem para o TOOLS.UTIL.API gravar no Azure Blob Storage.\n");

            // Send file and return HttpResponseMessage.
            var httpResponseMessage = await _externalUtil.SendFileAsync(new Refit.StreamPart(formFile.OpenReadStream(), formFile.FileName, formFile.ContentType));

            // Convert httpResponseMessage to ApiResponse.
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<FileResponse>>(await httpResponseMessage.Content.ReadAsStringAsync());

            // Response success.
            if (apiResponse.Sucesso) return new ApiResponse<object>(apiResponse.Sucesso, apiResponse.StatusCode, apiResponse.Dados, apiResponse.Notificacoes);

            // response error.
            return new ApiResponse<object>(apiResponse.Sucesso, apiResponse.StatusCode, null, apiResponse.Notificacoes);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
}
