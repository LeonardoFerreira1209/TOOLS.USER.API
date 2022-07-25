using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;

namespace APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;

public interface IPersonRepository
{
    /// <summary>
    /// Método responsavel por criar uma pessoa no banco de dados.
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<(bool success, Person person)> Create(PersonFastRequest personFastRequest, Guid userId);

    /// <summary>
    /// Método responsavel por recuperar uma pessoa por Id.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<(bool success, Person person)> Get(Guid personId);

    /// <summary>
    /// Método responsavel por Completar o registro de um usuário.
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    Task<(bool success, Person person)> CompleteRegister(PersonFullRequest personFullRequest);

    Task<(bool success, byte[] image)> ProfileImage(Person person, byte[] image);
}
