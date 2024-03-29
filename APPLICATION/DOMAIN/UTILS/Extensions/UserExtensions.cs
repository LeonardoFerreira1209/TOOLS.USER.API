﻿using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.DTOS.RESPONSE.USER;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

/// <summary>
/// Extensões de usuários.
/// </summary>
public static class UserExtensions
{
    /// <summary>
    /// Convert um user create para um userEntity.
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    public static UserEntity ToIdentityUser(this UserCreateRequest userRequest)
    {
        return new UserEntity
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Age = DateTime.Today.Year - DateTime.Parse(userRequest.BirthDay).Year,
            BirthDay = userRequest.BirthDay,
            PlanId = userRequest.PlanId,
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

    /// <summary>
    /// Convert um user update para um userEntity.
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    public static UserEntity ToCompleteUserUpdateWithRequest(this UserUpdateRequest userUpdateRequest, UserEntity user)
    {
        user.FirstName = userUpdateRequest.FirstName;
        user.LastName = userUpdateRequest.LastName;
        user.Age = DateTime.Today.Year - DateTime.Parse(userUpdateRequest.BirthDay).Year;
        user.Gender = userUpdateRequest.Gender;
        user.BirthDay = userUpdateRequest.BirthDay;
        user.CPF = userUpdateRequest.CPF;
        user.RG = userUpdateRequest.RG;
        user.Updated = DateTime.Now;
        user.UpdatedUserId = GlobalData.GlobalUser.Id;

        return user;
    }

    /// <summary>
    /// Convert um userEntity para um user response.
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    public static UserResponse ToResponse(this UserEntity user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PlanId = user.PlanId,
            Plan = user.Plan?.ToResponse(),
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Age = user.Age,
            BirthDay = user.BirthDay,
            CPF = user.CPF,
            RG = user.RG,
            Created = user.Created,
            Updated = user.Updated,
            UpdatedUserId = user.UpdatedUserId,
            CreatedUserId = user.CreatedUserId,
            Gender = user.Gender,
            ImageUri = user.ImageUri,
            Status = user.Status
        };
    }
}
