using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;
using APPLICATION.DOMAIN.UTILS;
using HotChocolate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TOOLS.USER.API.CONTROLLER.USER.CLAIM
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IUserService _userService;

        public ClaimController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/security/addclaim")]
        [Authorize(Policy = "User")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Remover claim do usuário", Description = "Método responsável por Remover claim do usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> AddClaim([Required] string username, [FromBody] ClaimRequest claimRequest)
        {
            using (LogContext.PushProperty("Controller", "ClaimController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(claimRequest)))
            using (LogContext.PushProperty("Metodo", "AddClaim"))
            {
                return await Tracker.Time(() => _userService.AddClaim(username, claimRequest), "Adicionar claim no usuário.");
            }
        }

        [HttpDelete("/security/removeclaim")]
        [Authorize(Policy = "User")]
        [EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Remover claim do usuário", Description = "Método responsável por Remover claim do usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> RemoveClaim([Required] string username, string roleName)
        {
            using (LogContext.PushProperty("Controller", "ClaimController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(roleName)))
            using (LogContext.PushProperty("Metodo", "RemoveClaim"))
            {
                return await Tracker.Time(() => _userService.AddRole(username, roleName), "Adicionar role no usuário.");
            }
        }
    }
}
