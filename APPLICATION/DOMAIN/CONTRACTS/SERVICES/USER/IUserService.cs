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
    public Task<ApiResponse<TokenJWT>> Create(UserRequest request);

    /// <summary>
    /// Método responsavel por ativar um usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ApiResponse<TokenJWT>> Activate(ActivateUserRequest request);

    /// <summary>
    /// Método responsavel por adicionar uma claim ao usuário.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claimRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<TokenJWT>> AddClaim(string username, ClaimRequest claimRequest);
}
