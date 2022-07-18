using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN
{
    public interface ITokenService
    {
        /// <summary>
        /// Criação do token.
        /// </summary>
        /// <returns></returns>
        Task<object> CreateJsonWebToken(string username);
    }
}
