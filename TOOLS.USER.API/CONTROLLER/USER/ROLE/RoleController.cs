using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS;
using APPLICATION.DOMAIN.UTILS.AUTH.CUSTOMAUTHORIZE.ATTRIBUTE;
using APPLICATION.ENUMS;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;

namespace TOOLS.USER.API.CONTROLLER.USER.ROLE
{
    [Route("api/[controller]")][ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;

        public RoleController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Método responsável por recuperar roles do usuário.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getUserRoles/{userId}")][CustomAuthorize(Claims.Role, "Get")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Recuperar roles do usuário", Description = "Método responsável por recuperar roles do usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> GetUserRoles(Guid userId)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(userId)))
            using (LogContext.PushProperty("Metodo", "GetUserRoles"))
            {
                return await Tracker.Time(() => _userService.GetUserRolesAsync(userId), "Recuperar roles do usuário.");
            }
        }

        /// <summary>
        /// Método responsável por recuperar todas as roles.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")][CustomAuthorize(Claims.Role, "Get")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Recuperar todas as roles", Description = "Método responsável por recuperar todas as roles")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> GetAll()
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Metodo", "GetAll"))
            {
                return await Tracker.Time(() => _roleService.GetAsync(), "Recuperar todas as roles.");
            }
        }

        /// <summary>
        /// Método responsável por adicionar uma role.
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        [HttpPost("addRole")][CustomAuthorize(Claims.Role, "Post")][EnableCors("CorsPolicy")]
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
                return await Tracker.Time(() => _roleService.CreateAsync(roleRequest), "Adicionar role.");
            }
        }

        /// <summary>
        /// Método responsável por adicionar uma lista de claims na role.
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        [HttpPost("addClaimToRole")][CustomAuthorize(Claims.Role, "Post")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Adicionar uma lista de claims na role", Description = "Método responsável por adicionar uma lista de claims na role.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> AddClaimsToRole(RoleRequest roleRequest)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(roleRequest)))
            using (LogContext.PushProperty("Metodo", "AddClaimsToRole"))
            {
                return await Tracker.Time(() => _roleService.AddClaimsAsync(roleRequest), "Adicionar claims em uma role.");
            }
        }

        /// <summary>
        ///  Método responsável por remover uma lista de claims na role.
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        [HttpDelete("removeClaimToRole")][CustomAuthorize(Claims.Role, "Delete")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Remover uma lista de claims na role", Description = "Método responsável por Remover uma lista de claims na role")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> RemoverClaimsToRole(RoleRequest roleRequest)
        {
            using (LogContext.PushProperty("Controller", "RoleController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(roleRequest)))
            using (LogContext.PushProperty("Metodo", "RemoverClaimToRole"))
            {
                return await Tracker.Time(() => _roleService.RemoveClaimsAsync(roleRequest), "Remover claims em uma role.");
            }
        }

        /// <summary>
        /// Adicionar uma role mo usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost("addRoleToUser")][CustomAuthorize(Claims.Role, "Post")][EnableCors("CorsPolicy")]
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
                return await Tracker.Time(() => _userService.AddRoleAsync(username, roleName), "Adicionar role no usuário.");
            }
        }

        /// <summary>
        /// Remover a role do usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpDelete("removeRoleToUser")][CustomAuthorize(Claims.Role, "Delete")][EnableCors("CorsPolicy")]
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
                return await Tracker.Time(() => _userService.RemoveRoleAsync(username, roleName), "Remover role do usuário.");
            }
        }
    }
}
