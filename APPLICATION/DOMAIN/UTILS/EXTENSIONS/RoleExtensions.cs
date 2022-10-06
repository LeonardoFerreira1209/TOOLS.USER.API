using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class RoleExtensions
{
    public static Role ToIdentityRole(this RoleRequest roleRequest)
    {
        return new Role
        {
            Name = roleRequest.Name,
            CompanyId = roleRequest.CompanyId,
            CreatedUserId = GlobalData<object>.GlobalUser.Id,
            Created = DateTime.Now,
            Status = Status.Active
        };
    }
}
