using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;

public interface IPersonRepository
{
    Task<(bool success, Person person)> Create(PersonFastRequest personFastRequest, Guid userId);
    Task<(bool success, Person person)> Get(Guid personId);
    Task<(bool success, Person person)> CompleteRegister(PersonFullRequest personFullRequest);
}
