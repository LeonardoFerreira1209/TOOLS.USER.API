using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TOOLS.USER.API.CONTROLLER.USER.CLAIM
{
    [Route("api/[controller]")] [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IUserService _userService;

        public ClaimController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Método responsável por adicionar uma claim no usuário
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimRequest"></param>
        /// <returns></returns>
        [HttpPost("addclaim")]
        [Authorize(Policy = "User")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Remover claim do usuário", Description = "Método responsável por Adicionar claim no usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ObjectResult> AddClaim([Required] string username, [FromBody] ClaimRequest claimRequest)
        {
            using (LogContext.PushProperty("Controller", "ClaimController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(claimRequest)))
            using (LogContext.PushProperty("Metodo", "AddClaim"))
            {
                return await Tracker.Time(() => _userService.AddClaim(username, claimRequest), "Adicionar claim no usuário.");
            }
        }

        /// <summary>
        /// Método responsável por remover um claim do usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpDelete("removeclaim")]
        [Authorize(Policy = "User")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Remover claim do usuário", Description = "Método responsável por Remover claim do usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ObjectResult> RemoveClaim([Required] string username, string claimName)
        {
            using (LogContext.PushProperty("Controller", "ClaimController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(claimName)))
            using (LogContext.PushProperty("Metodo", "RemoveClaim"))
            {
                return await Tracker.Time(() => _userService.AddRole(username, claimName), "Adicionar role no usuário.");
            }
        }
    }
}
