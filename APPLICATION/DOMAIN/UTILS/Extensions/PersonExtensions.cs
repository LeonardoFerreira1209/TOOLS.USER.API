﻿using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.UTILS.GLOBAL;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class PersonExtensions
{
    /// <summary>
    /// Convert request to Entity 
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static PersonEntity ToEntity(this PersonFastRequest personFastRequest, Guid userId, Guid? companyId = null)
    {
        return new PersonEntity
        {
            FirstName = personFastRequest.FirstName,

            LastName = personFastRequest.LastName,

            Gender = personFastRequest.Gender,

            CPF = personFastRequest.CPF,
            
            Created = DateTime.Now,

            CreatedUserId = userId,

            UserId = userId,

            CompanyId = companyId
        };
    }

    /// <summary>
    /// Convert Request to Entity
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    public static PersonEntity ToEntity(this PersonFullRequest personFullRequest)
    {
        return new PersonEntity
        {
            Id = personFullRequest.Id,

            FirstName = personFullRequest.FirstName,

            LastName = personFullRequest.LastName,

            Age = personFullRequest.Age,

            BirthDay = personFullRequest.BirthDay,

            Contacts = personFullRequest.Contacts?.Select(contact => contact.ToEntity()).ToList(),

            Gender = personFullRequest.Gender,

            RG = personFullRequest.RG,

            CPF = personFullRequest.CPF,

            ImageUri = personFullRequest.ImageUri,

            CompanyId = personFullRequest.CompanyId,

            Status = personFullRequest.Status,

            UserId = personFullRequest.UserId,

            UpdatedUserId = GlobalData<object>.GlobalUser.Id
        };
    }

    /// <summary>
    /// Convert Entity to Response.
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static PersonResponse ToResponse(this PersonEntity person)
    {
        return new PersonResponse
        {
            Id = person.Id,

            FirstName = person.FirstName,

            LastName = person.LastName,

            Age = person.Age,

            BirthDay = person.BirthDay,

            Gender = person.Gender,

            ImageUri = person.ImageUri,

            CompanyId = person.CompanyId,

            Company = person.Company.ToResponse(),

            Contacts = person.Contacts?.Select(contact => contact.ToResponse()).ToList(),

            RG = person.RG,

            CPF = person.CPF,

            UserId = person.UserId,

            User = person.User,

            Status = person.Status
        };
    }
}
