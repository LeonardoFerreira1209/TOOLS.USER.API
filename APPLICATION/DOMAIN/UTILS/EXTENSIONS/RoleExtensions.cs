using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
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
}
