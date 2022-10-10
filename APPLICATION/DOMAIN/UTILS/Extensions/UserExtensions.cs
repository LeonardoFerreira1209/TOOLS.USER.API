using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class UserExtensions
{
    public static User ToIdentityUser(this UserCreateRequest userRequest)
    {
        return new User
        {
            UserName = userRequest.UserName,
            Email = userRequest.Email,
            PhoneNumber = userRequest.PhoneNumber,
            PasswordHash = userRequest.Password,
            Created = DateTime.Now,
            Status = Status.Active
        };
    }

    public static User ToIdentityUser(this UserUpdateRequest userUpdateRequest)
    {
        return new User
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
