using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APPLICATION.DOMAIN.UTILS.AUTH.TOKEN;

public class JwtSecurityKey
{
    public static SymmetricSecurityKey Create(string secret) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
}
