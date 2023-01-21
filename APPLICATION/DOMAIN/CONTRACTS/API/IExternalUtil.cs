using Refit;

namespace APPLICATION.DOMAIN.CONTRACTS.API;

public interface IExternalUtil
{
    /// <summary>
    /// Método de envio de arquivo para a API de TOOLS.UTIL.API.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    [Multipart]
    [Post("/api/File/send/blobstorage")]
    Task<HttpResponseMessage> SendFileAsync([AliasAs("file")] StreamPart stream);
}
