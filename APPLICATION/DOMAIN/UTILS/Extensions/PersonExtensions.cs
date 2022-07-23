using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.UTILS.Extensions;

namespace APPLICATION.DOMAIN.UTILS.PERSON;

public static class PersonExtensions
{
    /// <summary>
    /// Convert request to Entity 
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static Person ToEntity(this PersonFastRequest personFastRequest, Guid userId)
    {
        return new Person
        {
            FirstName = personFastRequest.FirstName,
            LastName = personFastRequest.LastName,

            Gender = personFastRequest.Gender,

            CPF = personFastRequest.CPF,

            UserId = userId
        };
    }

    /// <summary>
    /// Convert Request to Entity
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    public static Person ToEntity(this PersonFullRequest personFullRequest)
    {
        return new Person
        {
            Id = personFullRequest.Id,
            FirstName = personFullRequest.FirstName,
            LastName = personFullRequest.LastName,
            Age = personFullRequest.Age,
            BirthDay = personFullRequest.BirthDay,

            Professions = personFullRequest.Professions.Select(profession => profession.ToIdentity()).ToList(),
            Contacts = personFullRequest.Contacts.Select(contact => contact.ToEntity()).ToList(),

            Gender = personFullRequest.Gender,

            RG = personFullRequest.RG,
            CPF = personFullRequest.CPF,

            UserId = personFullRequest.UserId
        };
    }

    /// <summary>
    /// Convert Entity to Response.
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static PersonResponse  ToResponse(this Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Age = person.Age,
            BirthDay = person.BirthDay.ToDateOnly(),

            Contacts = person.Contacts?.Select(contact => contact.ToResponse()).ToList(),

            CPF = person.CPF,
            Gender = person.Gender,

            Professions = person.Professions.Select(profession => profession.ToResponse()).ToList(),

            RG = person.RG,
            UserId = person.UserId,
        };
    }
}
