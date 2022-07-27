using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;

public interface IPersonService
{
    /// <summary>
    /// Método responsavel por criar uma pessoa.
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ObjectResult> Create(PersonFastRequest personFastRequest, Guid userId);

    /// <summary>
    /// Métodod responsável por recuperar uma pessoa por Id.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<ObjectResult> Get(Guid personId);

    /// <summary>
    /// Método reponsavel por recuperar todas as pessoas.
    /// </summary>
    /// <returns></returns>
    Task<ObjectResult> GetAll();

    /// <summary>
    /// Métodod responsavel por completar i cadastro de uma pessoa.
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    Task<ObjectResult> CompleteRegister(PersonFullRequest personFullRequest);

    /// <summary>
    /// Método responsavel por adicionar uma imagem de perfil no usuário.
    /// </summary>
    /// <param name="imagem"></param>
    /// <returns></returns>
    Task<ObjectResult> ProfileImage(Guid personId, IFormFile formFile);
}
