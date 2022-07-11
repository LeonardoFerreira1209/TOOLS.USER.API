using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.PROFILES.USER;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRequest, IdentityUser<Guid>>();
    }
}
