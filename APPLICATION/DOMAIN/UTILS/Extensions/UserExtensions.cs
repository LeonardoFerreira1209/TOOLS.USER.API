using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class UserExtensions
{
    public static IdentityUser<Guid> ToIdentityUser(this UserRequest userRequest)
    {
        return new IdentityUser<Guid>
        {
            UserName = userRequest.UserName,
            Email = userRequest.Email,
            PhoneNumber = userRequest.PhoneNumber,
            PasswordHash = userRequest.Password
        };
    }
}
