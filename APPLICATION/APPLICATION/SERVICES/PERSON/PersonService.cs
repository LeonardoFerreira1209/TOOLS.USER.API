using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PROFESSION;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;

namespace APPLICATION.APPLICATION.SERVICES.PERSON;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task Create(PersonFastRequest personFastRequest, Guid userId)
    {
        await _personRepository.Create(personFastRequest, userId);
    }

    public async Task<PersonResponse> CompleteRegister(PersonFullRequest personFullRequest)
    {
        var person = await _personRepository.CompleteRegister(personFullRequest);

        return new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Age = person.Age,
            //BirthDay = person.BirthDay.ToLocalTime(),

            Contacts = person.Contacts.Select(c => new ContactResponse
            {
                Name = c.Name,
                CEP = c.CEP,
                Complement = c.Complement,
                Email = c.Email,
                Number = c.Number,
                PersonId = c.PersonId,
                PhoneNumber = c.PhoneNumber

            }).ToList(),

            CPF = person.CPF,
            Gender = person.Gender,

            Professions = person.Professions.Select(p => new ProfessionResponse
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

            RG = person.RG,
            UserId = person.UserId,
        };
    }

    public Task<PersonResponse> ProfileImage(byte[] imagem)
    {
        throw new NotImplementedException();
    }
}
