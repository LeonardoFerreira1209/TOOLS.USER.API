using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.DOMAIN.VALIDATORS;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;
using APPLICATION.INFRAESTRUTURE.SIGNALR.CLIENTS;
using APPLICATION.INFRAESTRUTURE.SIGNALR.HUBS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace APPLICATION.APPLICATION.SERVICES.PERSON;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    private readonly IHubContext<HubPerson, IPersonClient> _hubPerson;

    public PersonService(IPersonRepository personRepository, IHubContext<HubPerson, IPersonClient> hubPerson)
    {
        _hubPerson = hubPerson;

        _personRepository = personRepository;
    }

    /// <summary>
    /// Método responsavel por criar uma pessoa.
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> Create(PersonFastRequest personFastRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Criando pessoa referente ao usuário.\n");

            // Create person
            var (success, person) = await _personRepository.Create(personFastRequest, userId);

            // Is success...
            if (success is true)
            {
                Log.Information($"[LOG INFORMATION] - Pessoa criada com sucesso.\n");

                // Response success.
                return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.SuccessCreated, null, new List<DadosNotificacao> { new DadosNotificacao("Pessoa criada com sucesso!") });
            }

            // Response error.
            return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao criar pessoa!") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }

    /// <summary>
    /// Métodod responsável por recuperar uma pessoa por Id.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> Get(Guid personId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(Get)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando uma pessoa.\n");

            // Get person for Id.
            var (success, person) = await _personRepository.Get(personId, true);

            // Is success or person is null.
            if (success is false || person is null)
            {
                // Person is null.
                if (person is null)
                {
                    Log.Information($"[LOG INFORMATION] - Pessoa não encontrada.\n");

                    // Response error.
                    return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.ErrorNotFound, person, new List<DadosNotificacao> { new DadosNotificacao("Pessoa não encontrada!") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao recuperar pessoa.\n");

                // Response error.
                return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, person, new List<DadosNotificacao> { new DadosNotificacao("Falha ao recuperar pessoa!") });
            }

            Log.Information($"[LOG INFORMATION] - Pessoa recuperada com sucesso.\n");

            // Response success
            return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.SuccessOK, person.ToResponse(), new List<DadosNotificacao> { new DadosNotificacao("Pessoa recuperada com sucesso!") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }

    /// <summary>
    /// Método responsavel por recuperar todas as pessoas.
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse<object>> GetAll()
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(GetAll)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando todas as pessosa.\n");

            // Get all persons.
            var (success, persons) = await _personRepository.GetAll(true);

            // Is success or persons is null.
            if (success is false || persons is null || persons.Any() is false)
            {
                // Persons is null.
                if (persons is null || persons.Any() is false)
                {
                    Log.Information($"[LOG INFORMATION] - Pessoas não encontrada.\n");

                    // Response error.
                    return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.ErrorNotFound , persons, new List<DadosNotificacao> { new DadosNotificacao("Pessoa não encontrada!") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao recuperar pessoas.\n");

                // Response error.
                return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.ErrorNotFound, null, new List<DadosNotificacao> { new DadosNotificacao("Falha ao recuperar pessoas!") });
            }

            Log.Information($"[LOG INFORMATION] - Pessoas recuperadas com sucesso.\n");

            // Response success
            return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.SuccessOK, persons.Select(person => person.ToResponse()), new List<DadosNotificacao> { new DadosNotificacao("Pessoa recuperada com sucesso!") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }

    /// <summary>
    /// Métodod responsavel por completar i cadastro de uma pessoa.
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    public async Task<ApiResponse<object>> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(CompleteRegister)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Completando registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

            var validation = await new CompletePersonValidator().ValidateAsync(personFullRequest); if (validation.IsValid is false) return validation.CarregarErrosValidator();

            // Complete Register.
            var (success, person) = await _personRepository.CompleteRegister(personFullRequest);

            // Is not success...
            if (success is false) return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao("Falaha ao completar registro de pessoa.") });

            Log.Information($"[LOG INFORMATION] - Registro de usuário completado com sucesso {person.FirstName} {person.LastName}.\n");

            // Response success.
            return new ApiResponse<object>(success, DOMAIN.ENUM.StatusCodes.SuccessOK, person, new List<DadosNotificacao> { new DadosNotificacao("Registro da pessoa completado com sucesso") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }

    /// <summary>
    /// Método responsavel por adicionar uma imagem de perfil no usuário.
    /// </summary>
    /// <param name="imagem"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ApiResponse<object>> ProfileImage(Guid personId, IFormFile formFile)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(ProfileImage)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando imagem na pessoa.\n");

            if (formFile.ContentType.FileTypesAllowed())
            {
                // Declare a memory stream.
                var memoryStream = new MemoryStream();

                // Copy formFile to memoryStream.
                await formFile.CopyToAsync(memoryStream);

                // Get a person.
                var (personSuccess, person) = await _personRepository.Get(personId, false);

                if (personSuccess && person is not null)
                {
                    // Add image in person.
                    var (imageSuccess, image) = await _personRepository.ProfileImage(person, memoryStream.ToArray());

                    // Is false.
                    if (imageSuccess is false)
                    {
                        Log.Information($"[LOG INFORMATION] - Falha ao adicionar imagem na pessoa.\n");

                        // Response error.
                        return new ApiResponse<object>(imageSuccess, DOMAIN.ENUM.StatusCodes.ErrorBadRequest, null, new List<DadosNotificacao> { new DadosNotificacao("Adicionar imagem na pessoa falhou.") });
                    }

                    Log.Information($"[LOG INFORMATION] - Imagem adicionada com sucesso na pessoa.\n");

                    // Response success.
                    return new ApiResponse<object>(imageSuccess, DOMAIN.ENUM.StatusCodes.SuccessOK, new FileContentResult(image, "image/jpg"), new List<DadosNotificacao> { new DadosNotificacao("Imagem adicionada com sucesso.") });
                }

                Log.Information($"[LOG INFORMATION] - Pessoa não foi encontrada.\n");

                // Response error.
                return new ApiResponse<object>(personSuccess, DOMAIN.ENUM.StatusCodes.ErrorNotFound, null, new List<DadosNotificacao> { new DadosNotificacao("Pessoa não encontada.") });
            }

            // Response error.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ErrorUnsupportedMediaType, null, new List<DadosNotificacao> { new DadosNotificacao("Tipo de arquivo não é permitido.") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // Error response.
            return new ApiResponse<object>(false, DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, null, new List<DadosNotificacao> { new DadosNotificacao(exception.Message) });
        }
    }
}
