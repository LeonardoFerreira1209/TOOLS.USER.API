using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TOOLS.USER.API.CONTROLLER.BASE;

[Route("api/[controller]")] [ApiController]
public class BaseController : ControllerBase
{
    [HttpOptions("/options")]
    public async Task<IActionResult> Options()
    {
        return await Task.FromResult(Ok("Retorno das options da aplicação!"));
    }
}
