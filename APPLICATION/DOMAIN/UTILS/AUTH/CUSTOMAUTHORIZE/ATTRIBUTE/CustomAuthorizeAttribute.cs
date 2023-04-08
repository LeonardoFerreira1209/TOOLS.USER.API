using APPLICATION.DOMAIN.UTILS.AUTH.CUSTOMAUTHORIZE.FILTER;
using APPLICATION.ENUMS;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace APPLICATION.DOMAIN.UTILS.AUTH.CUSTOMAUTHORIZE.ATTRIBUTE;

[ExcludeFromCodeCoverage]
public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Atributo de autorização customizavel.
    /// </summary>
    /// <param name="claim"></param>
    /// <param name="values"></param>
    public CustomAuthorizeAttribute(Claims claim, params string[] values) : base(typeof(CustomAuthorizeFilter))
    {
        Arguments = new object[] { values.Select(value => new Claim(claim.ToString(), value)).ToList() };
    }
}
