using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace APPLICATION.DOMAIN.UTILS.AUTH.CUSTOMAUTHORIZE.FILTER;

public class CustomAuthorizeFilter : IAuthorizationFilter
{
    private readonly List<Claim> _claims;

    /// <summary>
    /// Filtro de autorização customizavel.
    /// </summary>
    /// <param name="claims"></param>
    public CustomAuthorizeFilter(List<Claim> claims) => _claims = claims;

    /// <summary>
    /// Autorização customizavel.
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasClaim = context.HttpContext.User.Claims.Any(userClaim => _claims.Any(claim => userClaim.Type.Equals(claim.Type) && userClaim.Value.Equals(claim.Value)));

        if (hasClaim is false)
        {
            context.Result = new ObjectResult(new ApiResponse<object>(false, StatusCodes.ErrorForbidden, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário não têm permissão para acessar essa funcionalidade.") }));
        }
    }
}
