using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using Microsoft.AspNetCore.Mvc;

namespace TOOLS.USER.API.CONTROLLER.CONFIG;

[Route("api/[controller]")][ApiController]
public class ConfigController : ControllerBase
{
    [HttpOptions("options")]
    public async Task<ApiResponse<object>> Options()
    {
        return await Task.FromResult(new ApiResponse<object>(true, APPLICATION.DOMAIN.ENUM.StatusCodes.SuccessOK, new List<DadosNotificacao> { new DadosNotificacao("Headers suportados pela aplicação.") }));
    }
}
