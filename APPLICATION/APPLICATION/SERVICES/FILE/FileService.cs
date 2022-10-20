using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.FILE;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.RESPONSE.FILE;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.VALIDATORS;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;

namespace APPLICATION.APPLICATION.SERVICES.FILE;

public class FileService : IFileService
{
    private readonly IOptions<AppSettings> _appsettings;

    public FileService(IOptions<AppSettings> appsettings) { _appsettings = appsettings; }

    /// <summary>
    /// Gravar um arquivo no azure blob storage e retorna sua URL.
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> InviteFileToAzureBlobStorageAndReturnUri(IFormFile formFile)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(FileService)} - METHOD {nameof(InviteFileToAzureBlobStorageAndReturnUri)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando imagem no azure blob storage.\n");

            // Validade formFile.
            var validation = await new ImageUploadValidator().ValidateAsync(formFile); if (validation.IsValid is false) return validation.CarregarErrosValidator();

            // Declare a memory stream.
            var memoryStream = new MemoryStream();

            // Copy formFile to memoryStream.
            await formFile.CopyToAsync(memoryStream);

            // Create a azure blob client.
            var blobClient = new BlobClient(_appsettings.Value.AzureStorage.ConnectionStringAzureStorageKey, _appsettings.Value.AzureStorage.Container, formFile.FileName);

            // Upload file in azure blob storage.
            await blobClient.UploadAsync(formFile.OpenReadStream());

            Log.Information($"[LOG INFORMATION] - Imagem adicionada ao blob com sucesso, Url: {blobClient.Uri.AbsoluteUri}.\n");

            // Response error.
            return new ApiResponse<object>(true, DOMAIN.ENUM.StatusCodes.ErrorNotFound, new FileResponse { FileUri = blobClient.Uri.AbsoluteUri } , new List<DadosNotificacao> { new DadosNotificacao("Pessoa não encontada.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
}
