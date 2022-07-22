using APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PROFESSION;
using APPLICATION.DOMAIN.UTILS.PERSON;
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

    public async Task CompleteRegister(PersonFullRequest personFullRequest)
    {
        await _personRepository.CompleteRegister(personFullRequest);
    }

    public Task<PersonResponse> ProfileImage(byte[] imagem)
    {
        throw new NotImplementedException();
    }
}
