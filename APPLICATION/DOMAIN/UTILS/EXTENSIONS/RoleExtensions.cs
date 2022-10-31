using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.USER.ROLE;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class RoleExtensions
{
    public static RoleEntity ToIdentityRole(this RoleRequest roleRequest)
    {
        return new RoleEntity
        {
            Name = roleRequest.Name,
            CreatedUserId = GlobalData<object>.GlobalUser.Id,
            Created = DateTime.Now,
            Status = Status.Active
        };
    }

    public static RolesResponse ToResponse(this RoleEntity role)
    {
        return new RolesResponse
        {
            Name = role.Name,
            Created = role.Created,
            CreatedUserId = role.CreatedUserId,
            Updated = role.Updated,
            Status = role.Status,
            UpdatedUserId = role.UpdatedUserId
        };
    }
}
