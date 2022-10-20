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
    Task<(bool success, PersonEntity person)> Create(PersonFastRequest personFastRequest, Guid userId);

    /// <summary>
    /// Método responsavel por recuperar uma pessoa por Id.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<(bool success, PersonEntity person)> Get(Guid personId, bool withDependencies);

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
    Task<(bool success, IEnumerable<PersonEntity> persons)> GetAll(bool withDependencies);

    /// <summary>
    /// Método responsavel por Completar o registro de um usuário.
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    Task<(bool success, PersonEntity person)> CompleteRegister(PersonFullRequest personFullRequest);

    /// <summary>
    ///  Método responsavel por atualizar os dados do usuário.
    /// </summary>
    /// <param name="CompanyId"></param>
    /// <returns></returns>
    Task<(bool success, PersonEntity person)> Update(PersonEntity personEntity);

    /// <summary>
    /// Método responsavel por cadastrar a imagem de perfil do usuário.
    /// </summary>
    /// <param name="person"></param>
    /// <param name="imageUri"></param>
    /// <returns></returns>
    Task<(bool success, string imageUri)> ProfileImage(PersonEntity person, string imageUri);
}
