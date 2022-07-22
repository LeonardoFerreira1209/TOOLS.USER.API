using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
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

    public async Task Create(PersonFastRequest personFastRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(Create)}\n");

        await _contexto.Persons.AddAsync(personFastRequest.ToEntity(userId)); await _contexto.SaveChangesAsync();
    }

    public async Task CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(CompleteRegister)}\n");

        _contexto.Persons.Update(personFullRequest.ToEntity()); await _contexto.SaveChangesAsync();
    }
}
