﻿using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;

namespace TOOLS.USER.API.CONTROLLER.PERSON
{
    [Route("api/[controller]")] [ApiController]
    public class PersonController : ControllerBase
    {
        /// <summary>
        /// Método responsável por completar o cadastro de uma pessoa
        /// </summary>
        /// <param name="personFullRequets"></param>
        /// <returns></returns>
        [HttpPost("/completeRegister")]
        [Authorize(Policy = "User")][EnableCors("CorsPolicy")]
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
               
                return new ApiResponse<object>();

                //return await Tracker.Time(() => _userService.AddClaim(username, claimRequest), "Adicionar claim no usuário.");
            }
        }

        /// <summary>
        /// Método responsável por adicionar uma imagem de perfil em uma pessoa.
        /// </summary>
        /// <param name="profileImage"></param>
        /// <returns></returns>
        [HttpPost("/completeRegister")]
        [Authorize(Policy = "User")][EnableCors("CorsPolicy")]
        [SwaggerOperation(Summary = "Adicionar imagem de perfil na Pessoa", Description = "Método responsável por adicionar uma imagem de perfil em uma pessoa.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<object>> ProfileImage(IFormFile profileImage)
        {
            using (LogContext.PushProperty("Controller", "PersonController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(profileImage)))
            using (LogContext.PushProperty("Metodo", "ProfileImage"))
            {
                MemoryStream memoryStream = new();

                await profileImage.CopyToAsync(memoryStream);
                
                byte[] bytes = memoryStream.ToArray();

                var imagem = bytes;

                return new ApiResponse<object>();

                //return await Tracker.Time(() => _userService.AddClaim(username, claimRequest), "Adicionar claim no usuário.");
            }
        }
    }
}