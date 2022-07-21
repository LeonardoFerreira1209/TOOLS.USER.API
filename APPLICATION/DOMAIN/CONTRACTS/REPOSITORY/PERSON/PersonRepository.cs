using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
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

        var person = new Person
        {
            FirstName = personFastRequest.FirstName,
            LastName = personFastRequest.LastName,
            Gender = personFastRequest.Gender,
            CPF = personFastRequest.CPF,
            UserId = userId
        };

        await _contexto.Persons.AddAsync(person);

        await _contexto.SaveChangesAsync();
    }

    public async Task<Person> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(CompleteRegister)}\n");

        var person = new Person
        {
            Id = personFullRequest.Id,
            FirstName = personFullRequest.FirstName,
            LastName = personFullRequest.LastName,
            Age = personFullRequest.Age,
            //BirthDay = personFullRequest.BirthDay,
            Contacts = personFullRequest.Contacts.Select(c => new Contact
            {
                Name = c.Name,
                CEP = c.CEP,
                Complement = c.Complement,
                Email = c.Email,
                Number = c.Number,
                PersonId = c.PersonId,
                PhoneNumber = c.PhoneNumber

            }).ToList(),
            CPF = personFullRequest.CPF,
            Gender = personFullRequest.Gender,
            Professions = personFullRequest.Professions.Select(p => new Profession
            {
                CompanyId = p.CompanyId,
                Description = p.Description,
                PersonId = p.PersonId,
                //StartDate = p.StartDate,
                //EndDate = p.EndDate,
                Office = p.Office,
                Wage = p.Wage,
               //Workload = p.Workload,

            }).ToList(),
            RG = personFullRequest.RG,
            UserId = personFullRequest.UserId
        };

        _contexto.Persons.Update(person);

        await _contexto.SaveChangesAsync();

        return person;
    }
}
