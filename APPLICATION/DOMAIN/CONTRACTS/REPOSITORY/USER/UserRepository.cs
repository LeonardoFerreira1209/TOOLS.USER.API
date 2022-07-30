using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.BASE;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.USER;
using Microsoft.Extensions.Options;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.USER;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IOptions<AppSettings> appssetings) : base(appssetings) { }

    #region EF Core

    #endregion

    #region Dapper
    public async Task DeleteUserWithoutPerson()
    {
        // Retorna Ids de usuários que não têm vinculos com pessoas.
        var query = @"SELECT U.ID FROM ASPNETUSERS U (NOLOCK)
                WHERE NOT EXISTS (SELECT * FROM  PERSONS P WHERE U.ID = P.USERID)";

        var userIds = await DbQueryAsync<Guid>(GerarConexaoConnect(), query, null);

        // Deleta uma lista de usuários com base no Id.
        await DbExecuteAsync<bool>(GerarConexaoConnect(), @"DELETE FROM ASPNETUSERS WHERE ID IN (@userIds)", new { userIds });
    }
    #endregion
}
