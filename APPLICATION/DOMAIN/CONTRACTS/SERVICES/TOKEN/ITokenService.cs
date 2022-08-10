using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN
{
    public interface ITokenService
    {
        /// <summary>
        /// Criação do token.
        /// </summary>
        /// <returns></returns>
        Task<ApiResponse<object>> CreateJsonWebToken(string username);
    }
}
