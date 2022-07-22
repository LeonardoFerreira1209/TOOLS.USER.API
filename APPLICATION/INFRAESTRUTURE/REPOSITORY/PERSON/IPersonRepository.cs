using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;

public interface IPersonRepository
{
    Task Create(PersonFastRequest personFastRequest, Guid userId);
    Task CompleteRegister(PersonFullRequest personFullRequest);
}
