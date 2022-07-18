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

namespace TOOLS.USER.API.CONTROLLER.USER.ROLE
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;

        public RoleController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpPost("/security/addRole")]
        [Authorize(Policy = "User")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Adicionar role", Description = "Método responsável por Adicionar uma role")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> AddRole(RoleRequest roleRequest)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(roleRequest)))
            using (LogContext.PushProperty("Metodo", "AddRole"))
            {
                return await Tracker.Time(() => _roleService.Create(roleRequest), "Adicionar role.");
            }
        }

        [HttpPost("/security/addClaimToRole")]
        [Authorize(Policy = "User")]
        [EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Adicionar uma claim na role", Description = "Método responsável por Adicionar uma claim na role")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> AddClaimToRole([Required] string roleName, List<ClaimRequest> claimRequests)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(claimRequests)))
            using (LogContext.PushProperty("Metodo", "AddClaimToRole"))
            {
                return await Tracker.Time(() => _roleService.AddClaim(roleName, claimRequests), "Adicionar claim em uma role.");
            }
        }

        [HttpPost("/security/addRoleToUser")]
        [Authorize(Policy = "User")]
        [EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Adicionar role no usuário", Description = "Método responsável por Adicionar uma role no usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> AddRoleToUser([Required] string username, string roleName)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(roleName)))
            using (LogContext.PushProperty("Metodo", "AddRoleToUser"))
            {
                return await Tracker.Time(() => _userService.AddRole(username, roleName), "Adicionar role no usuário.");
            }
        }

        [HttpDelete("/security/removeRoleToUser")]
        [Authorize(Policy = "User")]
        [EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Remover role do usuário", Description = "Método responsável por Remover uma role do usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> RemoveRoleToUser([Required] string username, string roleName)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(roleName)))
            using (LogContext.PushProperty("Metodo", "RemoveRoleToUser"))
            {
                return await Tracker.Time(() => _userService.RemoveRole(username, roleName), "Remover role do usuário.");
            }
        }
    }
}
