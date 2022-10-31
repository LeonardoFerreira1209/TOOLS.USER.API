using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS;
using APPLICATION.DOMAIN.UTILS.AUTH;
using APPLICATION.ENUMS;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TOOLS.USER.API.CONTROLLER.USER.USER
{
    [Route("api/[controller]")][ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) { _userService = userService; }

        /// <summary>
        /// Método responsável por adicionar um usuario.
        /// </summary>
        /// <param name="userCreateRequest"></param>
        /// <returns></returns>
        [HttpPost("create")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Criar uauário.", Description = "Método responsavel por criar usuário")]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> Create(UserCreateRequest userCreateRequest)
        {
            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(userCreateRequest)))
            using (LogContext.PushProperty("Metodo", "Create"))
            {
                return await Tracker.Time(() => _userService.Create(userCreateRequest), "Criar usuário");
            }
        }

        /// <summary>
        /// Método responsável por atualizar um  usuario.
        /// </summary>
        /// <param name="userUpdateRequest"></param>
        /// <returns></returns>
        [HttpPut("update")][CustomAuthorize(Claims.User, "Put")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Atualizar uauário.", Description = "Método responsavel por atualizar usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> Update(UserUpdateRequest userUpdateRequest)
        {
            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(userUpdateRequest)))
            using (LogContext.PushProperty("Metodo", "Create"))
            {
                return await Tracker.Time(() => _userService.Update(userUpdateRequest), "Atualizar usuário");
            }
        }

        /// <summary>
        /// Método responsável por Ativar usuário.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("authetication")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Autenticação do usuário", Description = "Método responsável por Autenticar usuário")]
        [ProducesResponseType(typeof(ApiResponse<TokenJWT>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status423Locked)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> Authentication([FromHeader][Required] string username, [FromHeader][Required] string password)
        {
            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(new {username, password })))
            using (LogContext.PushProperty("Metodo", "Authentication"))
            {
                return await Tracker.Time(() => _userService.Authentication(new LoginRequest(username, password)), "Autenticar usuário");
            }
        }

        /// <summary>
        /// Recuperar um usuário.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        [CustomAuthorize(Claims.User, "Get")]
        [EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Recuperar um usuário", Description = "Método responsável por Recuperar um usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> Get(Guid userId)
        {
            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(userId)))
            using (LogContext.PushProperty("Metodo", "Get"))
            {
                return await Tracker.Time(() => _userService.Get(userId), "Recuperar um usuário");
            }
        }

        /// <summary>
        /// Método responsável por Ativar usuário.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("activate/{code}/{userId}")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Ativar usuário", Description = "Método responsável por Ativar usuário")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> Activate(string code, Guid userId)
        {
            var request = new ActivateUserRequest(code, userId);

            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(request)))
            using (LogContext.PushProperty("Metodo", "activate"))
            {
                return await Tracker.Time(() => _userService.Activate(request), "Ativar usuário");
            }
        }
    }
}
