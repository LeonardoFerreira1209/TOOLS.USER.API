using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
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
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
        }
    }

    public async Task<ApiResponse<object>> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonService)} - METHOD {nameof(CompleteRegister)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Completando registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

            var sucesso = await _personRepository.CompleteRegister(personFullRequest);

            if (sucesso is false)
            {
                Log.Information($"[LOG INFORMATION] - Falha ao finalizar registro da pessoa {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

                return new ApiResponse<object>(sucesso, null, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, "Registro da pessoa falhou.") });
            }

            Log.Information($"[LOG INFORMATION] - Registro de usuário completado com sucesso {personFullRequest.FirstName} {personFullRequest.LastName}.\n");

            return new ApiResponse<object>(sucesso, personFullRequest, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.SuccessCreated, "Registro da pessoa completado com sucesso") });
        }
        catch (Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return new ApiResponse<object>(false, new List<DadosNotificacao> { new DadosNotificacao(DOMAIN.ENUM.StatusCodes.ServerErrorInternalServerError, exception.Message) });
        }
    }

    public Task<PersonResponse> ProfileImage(byte[] imagem)
    {
        throw new NotImplementedException();
    }
}
