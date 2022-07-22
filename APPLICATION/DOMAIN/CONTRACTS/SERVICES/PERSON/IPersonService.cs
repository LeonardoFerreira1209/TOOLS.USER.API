using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.PERSON;

public interface IPersonService
{
    Task<ApiResponse<object>> Create(PersonFastRequest personFastRequest, Guid userId);

    Task<ApiResponse<object>> CompleteRegister(PersonFullRequest personFullRequest);

    Task<PersonResponse> ProfileImage(byte[] imagem);
}
