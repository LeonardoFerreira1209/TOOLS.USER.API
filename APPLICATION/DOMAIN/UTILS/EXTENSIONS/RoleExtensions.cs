using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class RoleExtensions
{
    public static IdentityRole<Guid> ToIdentityRole(this RoleRequest roleRequest)
    {
        return new IdentityRole<Guid>
        {
            Name = roleRequest.Name,
        };
    }
}
