using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using Microsoft.AspNetCore.Identity;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class UserExtensions
{
    public static IdentityUser<Guid> ToIdentityUser(this UserCreateRequest userRequest)
    {
        return new IdentityUser<Guid>
        {
            UserName = userRequest.UserName,
            Email = userRequest.Email,
            PhoneNumber = userRequest.PhoneNumber,
            PasswordHash = userRequest.Password
        };
    }

    public static IdentityUser<Guid> ToIdentityUser(this UserUpdateRequest userUpdateRequest)
    {
        return new IdentityUser<Guid>
        {
            Id = userUpdateRequest.Id,
            UserName = userUpdateRequest.UserName,
            Email = userUpdateRequest.Email,
            EmailConfirmed = userUpdateRequest.EmailConfirmed,
            PhoneNumber = userUpdateRequest.PhoneNumber,
            PhoneNumberConfirmed = userUpdateRequest.PhoneNumberConfirmed,
            SecurityStamp = userUpdateRequest.SecurityStamp,
            ConcurrencyStamp = userUpdateRequest.ConcurrencyStamp,
            AccessFailedCount = userUpdateRequest.AccessFailedCount,
            LockoutEnabled = userUpdateRequest.LockoutEnabled,
            LockoutEnd = userUpdateRequest.LockoutEnd,
            TwoFactorEnabled = userUpdateRequest.TwoFactorEnabled
        };
    }
}
