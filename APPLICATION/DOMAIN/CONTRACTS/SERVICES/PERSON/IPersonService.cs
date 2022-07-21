using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;

public interface IPersonService
{
    Task Create(PersonFastRequest personFastRequest, Guid userId);

    Task<PersonResponse> CompleteRegister(PersonFullRequest personFullRequest);

    Task<PersonResponse> ProfileImage(byte[] imagem);
}
