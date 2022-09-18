using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS;
using APPLICATION.DOMAIN.UTILS.AUTH;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;

namespace TOOLS.USER.API.CONTROLLER.PERSON;

[Route("api/[controller]")][ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService) { _personService = personService; }

    /// <summary>
    /// Método responsavel por personId.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    [HttpGet("get/{personId}")][CustomAuthorize(Claims.Person, "Get")][EnableCors("CorsPolicy")]
    [SwaggerOperation(Summary = "Recuperar uma pessoa", Description = "Método responsável por recuperar uma pessoa")]
    [ProducesResponseType(typeof(ApiResponse<PersonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ApiResponse<object>> Get(Guid personId)
    {
        using (LogContext.PushProperty("Controller", "PersonController"))
        using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(personId)))
        using (LogContext.PushProperty("Metodo", "Get"))
        {
            return await Tracker.Time(() => _personService.Get(personId), "Recuperar uma pessoa através.");
        }
    }

    /// <summary>
    /// Método responsavel por recuperar todas as pessoas.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    [HttpGet("getAll")][CustomAuthorize(Claims.Person, "Get")][EnableCors("CorsPolicy")]
    [SwaggerOperation(Summary = "Recuperar todas as pessoas", Description = "Método responsável por recuperar todas as pessoa")]
    [ProducesResponseType(typeof(ApiResponse<List<PersonResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ApiResponse<object>> GetAll()
    {
        using (LogContext.PushProperty("Controller", "PersonController"))
        using (LogContext.PushProperty("Payload", null))
        using (LogContext.PushProperty("Metodo", "GetAll"))
        {
            return await Tracker.Time(() => _personService.GetAll(), "Recuperar todas as pessoa através.");
        }
    }

    /// <summary>
    /// Método responsável por completar o cadastro de uma pessoa
    /// </summary>
    /// <param name="personFullRequets"></param>
    /// <returns></returns>
    [HttpPut("completeRegister")][CustomAuthorize(Claims.Person, "Put")][EnableCors("CorsPolicy")]
    [SwaggerOperation(Summary = "Completar cadastro da Pessoa", Description = "Método responsável por completar o cadastro de uma pessoa")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ApiResponse<object>> CompleteRegister(PersonFullRequest personFullRequets)
    {
        using (LogContext.PushProperty("Controller", "PersonController"))
        using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(personFullRequets)))
        using (LogContext.PushProperty("Metodo", "CompleteRegister"))
        {
            return await Tracker.Time(() => _personService.CompleteRegister(personFullRequets), "Completar cadastro de pessoa.");
        }
    }

    /// <summary>
    /// Método responsável por adicionar uma imagem de perfil em uma pessoa.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="formFile"></param>
    /// <returns></returns>
    [HttpPatch("profileImage/{personId}")][CustomAuthorize(Claims.Person, "Patch")][EnableCors("CorsPolicy")]
    [SwaggerOperation(Summary = "Adicionar imagem de perfil na Pessoa", Description = "Método responsável por adicionar uma imagem de perfil em uma pessoa.")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ApiResponse<object>> ProfileImage(Guid personId, IFormFile formFile)
    {
        using (LogContext.PushProperty("Controller", "PersonController"))
        using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(formFile)))
        using (LogContext.PushProperty("Metodo", "ProfileImage"))
        {
            return await Tracker.Time(() => _personService.ProfileImage(personId, formFile ??= Request.Form.Files.FirstOrDefault()), "Adicionar imagem na pessoa.");
        }
    }
}
