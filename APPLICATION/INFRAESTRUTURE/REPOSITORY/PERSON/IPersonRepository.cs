using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;

public interface IPersonRepository
{
    Task<bool> Create(PersonFastRequest personFastRequest, Guid userId);
    Task<bool> CompleteRegister(PersonFullRequest personFullRequest);
}
