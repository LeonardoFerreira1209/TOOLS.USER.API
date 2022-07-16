using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.PROFILES.USER
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleRequest, IdentityRole<Guid>>();
        }
    }
}
