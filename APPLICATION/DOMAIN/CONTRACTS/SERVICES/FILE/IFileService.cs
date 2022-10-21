using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using Microsoft.AspNetCore.Http;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.FILE;

public interface IFileService
{
    Task<ApiResponse<object>> InviteFileToAzureBlobStorageAndReturnUri(IFormFile formFile);
}
