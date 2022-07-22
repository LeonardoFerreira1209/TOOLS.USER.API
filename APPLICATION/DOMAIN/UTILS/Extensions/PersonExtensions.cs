using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.UTILS.Extensions;

namespace APPLICATION.DOMAIN.UTILS.PERSON;

public static class PersonExtensions
{
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

    public static Person ToEntity(this PersonFullRequest personFullRequest)
    {
        return new Person
        {
            FirstName = personFullRequest.FirstName,
            LastName = personFullRequest.LastName,
            Age = personFullRequest.Age,
            BirthDay = personFullRequest.BirthDay.ToDateTime(),
            //Professions = personFullRequest.Professions.Select(profession => profession.ToIdentity())

            Gender = personFullRequest.Gender,

            CPF = personFullRequest.CPF,

            UserId = personFullRequest.UserId
        };
    }

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
