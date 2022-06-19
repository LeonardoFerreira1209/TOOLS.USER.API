using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE;

namespace APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;

public interface IUserService
{
    /// <summary>
    /// Método responsável por fazer a autenticação do usuário
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<TokenJWT>> Authentication(LoginRequest request);

    /// <summary>
    /// Método responsável por criar um novo usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<TokenJWT>> Create(CreateRequest request);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<TokenJWT>> Activate(ActivateUserRequest request);
}
