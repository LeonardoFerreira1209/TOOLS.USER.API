using APPLICATION.DOMAIN.DTOS.RESPONSE;
using Microsoft.AspNetCore.Mvc;

namespace TOOLS.USER.API.CONTROLLER.CONFIG;

[Route("api/[controller]")][ApiController]
public class ConfigController : ControllerBase
{
    [HttpOptions("/options")]
    public async Task<ApiResponse<object>> Options()
    {
        return await Task.FromResult(new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(APPLICATION.DOMAIN.ENUM.StatusCodes.SuccessAccepted, "Headers suportados pela aplicação.") }));
    }
}
