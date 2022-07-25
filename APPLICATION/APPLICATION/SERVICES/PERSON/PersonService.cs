using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.UTILS.PERSON;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;
using Serilog;

namespace APPLICATION.APPLICATION.SERVICES.PERSON;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<ApiResponse<object>> Create(PersonFastRequest personFastRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Criando pessoa referente ao usuário.\n");

            await _personRepository.Create(personFastRequest, userId);

            Log.Information($"[LOG INFORMATION] - Pessoa criada com sucesso.\n");

            return new ApiResponse<object>(true, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Pessoa criada com sucesso!") });

        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
        }
    }

    public async Task<ApiResponse<object>> Get(Guid personId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(Get)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Recuperando uma pessoa.\n");

            var (success, person) = await _personRepository.Get(personId);

            if(success is false || person is null)
            {
                if(person is null)
                {
                    Log.Information($"[LOG INFORMATION] - Pessoa não encontrada.\n");

                    return new ApiResponse<object>(success, person, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Pessoa não encontrada!") });
                }

                Log.Information($"[LOG INFORMATION] - Falha ao recuperar pessoa.\n");

                return new ApiResponse<object>(success, person, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, " Falha ao recuperar pessoa!") });
            }

            Log.Information($"[LOG INFORMATION] - Pessoa recuperada com sucesso.\n");

            return new ApiResponse<object>(success, person.ToResponse(), new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Pessoa recuperada com sucesso!") });

        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
        }
    }

    public async Task<ApiResponse<object>> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(CompleteRegister)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Completando registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

            var (success, person) = await _personRepository.CompleteRegister(personFullRequest);

            if (success is false)
            {
                Log.Information($"[LOG INFORMATION] - Falha ao finalizar registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

                return new ApiResponse<object>(success, null, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, "Registro da pessoa falhou.") });
            }

            Log.Information($"[LOG INFORMATION] - Registro de usuário completado com sucesso {person.FirstName} {person.LastName}.\n");

            return new ApiResponse<object>(success, person, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Registro da pessoa completado com sucesso") });
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
        }
    }

    public Task<PersonResponse> ProfileImage(byte[] imagem)
    {
        throw new NotImplementedException();
    }
}
