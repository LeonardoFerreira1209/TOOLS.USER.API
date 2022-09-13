using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.DOMAIN.ENUM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace APPLICATION.DOMAIN.UTILS.AUTH.CUSTOMAUTHORIZE.FILTER;

public class CustomAuthorizeFilter : IAuthorizationFilter
{
    private readonly List<Claim> _claims;

    public CustomAuthorizeFilter(List<Claim> claims) => _claims = claims;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasClaim = false;

        foreach (var userClaim in context.HttpContext.User.Claims)
        {
            if (_claims.Where(claim => userClaim.Type.Equals(claim.Type) && userClaim.Value.Equals(claim.Value)).Any()) { hasClaim = true; break; }
        }

        if (hasClaim is false)
        {
            context.Result = new ObjectResult(new ApiResponse<object>(false, StatusCodes.ErrorForbidden, null, new List<DadosNotificacao> { new DadosNotificacao("Usuário não têm permissão para acessar essa funcionalidade.") }));
        }
    }
}
