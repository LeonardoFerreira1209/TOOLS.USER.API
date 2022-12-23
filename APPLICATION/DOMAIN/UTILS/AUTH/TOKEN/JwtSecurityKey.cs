using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace APPLICATION.DOMAIN.UTILS.AUTH.TOKEN;

[ExcludeFromCodeCoverage]
public static class JwtSecurityKey
{
    public static SymmetricSecurityKey Create(string secret) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
}
