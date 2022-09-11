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
    Task<(bool success, Person person)> Get(Guid personId, bool withDependencies);

    /// <summary>
    /// Método responsável por recuperar o Id de uma pessoa pelo userId.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<(bool success, Guid personId)> GetIdWithUserId(Guid userId);

    /// <summary>
    /// Método responsavel por recuperar todas as pessoas.
    /// </summary>
    /// <param name="withDependencies"></param>
    /// <returns></returns>
    Task<(bool success, IEnumerable<Person> persons)> GetAll(bool withDependencies);

    /// <summary>
    /// Método responsavel por Completar o registro de um usuário.
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    Task<(bool success, Person person)> CompleteRegister(PersonFullRequest personFullRequest);

    /// <summary>
    /// Métodor responsavel por cadastrar a imagem de perfil do usuário.
    /// </summary>
    /// <param name="person"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    Task<(bool success, byte[] image)> ProfileImage(Person person, byte[] image);
}
