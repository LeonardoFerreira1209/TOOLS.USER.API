using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using APPLICATION.DOMAIN.UTILS.PERSON;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace APPLICATION.APPLICATION.SERVICES.PERSON;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<ObjectResult> Create(PersonFastRequest personFastRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Criando pessoa referente ao usuário.\n");

            // Create person
            await _personRepository.Create(personFastRequest, userId);

            Log.Information($"[LOG INFORMATION] - Pessoa criada com sucesso.\n");

            // Response success.
            var apiResponseSuccess = new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Pessoa criada com sucesso!") });

            // Return response.
            return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessCreated};

        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

            // Return error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
        }
    }

    public async Task<ObjectResult> Get(Guid personId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(Get)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando uma pessoa.\n");

            // Get person for Id.
            var (success, person) = await _personRepository.Get(personId);

            // Is success or person is null.
            if(success is false || person is null)
            {
                // Person is null.
                if(person is null)
                {
                    Log.Information($"[LOG INFORMATION] - Pessoa não encontrada.\n");

                    // Response error.
                    var apiResponseError =  new ApiResponse<object>(success, person, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorNotFound, "Pessoa não encontrada!") });

                    // Return response.
                    return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ErrorNotFound };
                }

                Log.Information($"[LOG INFORMATION] - Falha ao recuperar pessoa.\n");

                // Response error.
                var apiResponseErrorFailed = new ApiResponse<object>(success, person, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, "Falha ao recuperar pessoa!") });

                // Return response erro.
                return new ObjectResult(apiResponseErrorFailed) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
            }

            Log.Information($"[LOG INFORMATION] - Pessoa recuperada com sucesso.\n");

            // Response success
            var apiResponseSuccess = new ApiResponse<object>(success, person.ToResponse(), new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, "Pessoa recuperada com sucesso!") });

            // Return repsonse.
            return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

            // Return error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
        }
    }

    public async Task<ObjectResult> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(CompleteRegister)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Completando registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

            // Complete Register.
            var (success, person) = await _personRepository.CompleteRegister(personFullRequest);

            // Is not success
            if (success is false)
            {
                Log.Information($"[LOG INFORMATION] - Falha ao finalizar registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

                // Response error.
                var apiResponseError = new ApiResponse<object>(success, null, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ErrorBadRequest, "Registro da pessoa falhou.") });

                // Return error.
                return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ErrorBadRequest };
            }

            Log.Information($"[LOG INFORMATION] - Registro de usuário completado com sucesso {person.FirstName} {person.LastName}.\n");

            // Response success.
            var apiResponseSuccess = new ApiResponse<object>(success, person, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessOK, "Registro da pessoa completado com sucesso") });

            // Return response.
            return new ObjectResult(apiResponseSuccess) { StatusCode = (int)StatusCodes.SuccessOK };
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Error response.
            var apiResponseError = new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(StatusCodes.ServerErrorInternalServerError, exception.Message) });

            // Return error.
            return new ObjectResult(apiResponseError) { StatusCode = (int)StatusCodes.ServerErrorInternalServerError };
        }
    }

    public Task<PersonResponse> ProfileImage(byte[] imagem)
    {
        throw new NotImplementedException();
    }
}
