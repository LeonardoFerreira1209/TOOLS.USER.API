using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class UserExtensions
{
    public static UserEntity ToIdentityUser(this UserCreateRequest userRequest)
    {
        return new UserEntity
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Age = userRequest.Age,
            BirthDay = userRequest.BirthDay,
            CompanyId = userRequest.CompanyId,
            CPF = userRequest.CPF,
            RG = userRequest.RG,
            Gender = userRequest.Gender,
            ImageUri = userRequest.ImageUri,
            UserName = userRequest.UserName,
            Email = userRequest.Email,
            PhoneNumber = userRequest.PhoneNumber,
            PasswordHash = userRequest.Password,
            Created = DateTime.Now,
            Status = Status.Active
        };
    }

    public static UserEntity ToCompleteUserUpdateWithRequest(this UserUpdateRequest userUpdateRequest, UserEntity user)
    {
        user.UserName = userUpdateRequest.UserName;
        user.FirstName = userUpdateRequest.FirstName;
        user.LastName = userUpdateRequest.LastName;
        user.Age = userUpdateRequest.Age;
        user.Gender = userUpdateRequest.Gender;
        user.BirthDay = userUpdateRequest.BirthDay;
        user.CPF = userUpdateRequest.CPF;
        user.RG = userUpdateRequest.RG;
        user.Email = userUpdateRequest.Email;
        user.PhoneNumber = userUpdateRequest.PhoneNumber;
        user.Updated = DateTime.Now;

        user.UpdatedUserId = GlobalData<object>.GlobalUser.Id;

        return user;
    }
}
