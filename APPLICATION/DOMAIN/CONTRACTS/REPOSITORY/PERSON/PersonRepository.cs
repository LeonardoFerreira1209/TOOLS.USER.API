using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.UTILS.PERSON;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;
using Microsoft.Extensions.Options;
using Serilog;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.PERSON;

public class PersonRepository : IPersonRepository
{
    private readonly Contexto _contexto;

    private readonly IOptions<AppSettings> _appSettings;

    public PersonRepository(IOptions<AppSettings> appSettings, Contexto contexto)
    {
        _contexto = contexto;

        _appSettings = appSettings;
    }

    public async Task<bool> Create(PersonFastRequest personFastRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(Create)}\n");

        try
        {
            await _contexto.Persons.AddAsync(personFastRequest.ToEntity(userId)); await _contexto.SaveChangesAsync();

            return true;
        }
        catch(Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return false;
        }
    }

    public async Task<bool> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(CompleteRegister)}\n");

        try
        {
            _contexto.Persons.Update(personFullRequest.ToEntity()); await _contexto.SaveChangesAsync();

            return true;
        }
        catch(Exception exception)
        {
            Log.Error("[LOG ERROR]", exception, exception.Message);

            return false;
        }
    }
}
