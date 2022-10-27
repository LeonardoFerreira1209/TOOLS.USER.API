using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using Microsoft.AspNetCore.Http;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.FILE;

public interface IFileService
{
    /// <summary>
    /// Emvio de arquivos para o blob storage do Azure.
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> InviteFileToAzureBlobStorageAndReturnUri(IFormFile formFile);
}
