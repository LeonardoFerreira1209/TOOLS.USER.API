using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;

public interface ITokenService
{
    /// <summary>
    /// Criação do token.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<(TokenJWT tokenJWT, List<DadosNotificacao> messages)> CreateJsonWebToken(string username);
}
