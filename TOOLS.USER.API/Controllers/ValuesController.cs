using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace TOOLS.USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [EnableCors("CorsPolicy")]
        [HttpGet("/teste/security/activate/{codigo}/{usuarioId}")]
        public async Task<IActionResult> teste(string codigo, int usuarioId)
        {
            return Ok("Funcionou");
        }

        [EnableCors("CorsPolicy")]
        [HttpOptions]
        public async Task<IActionResult> options()
        {
            return Ok("Funcionou");
        }
    }
}
