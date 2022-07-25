using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using Microsoft.AspNetCore.Mvc;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;

public interface IPersonService
{
    Task<ObjectResult> Create(PersonFastRequest personFastRequest, Guid userId);

    Task<ObjectResult> Get(Guid userId);

    Task<ObjectResult> CompleteRegister(PersonFullRequest personFullRequest);

    Task<PersonResponse> ProfileImage(byte[] imagem);
}
