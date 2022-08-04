using APPLICATION.DOMAIN.DTOS.REQUEST.CONTACT;
using APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;
using APPLICATION.DOMAIN.ENTITY.CONTACT;

namespace APPLICATION.DOMAIN.UTILS.EXTENSIONS;

public static class ContactExtensions
{
    public static ContactResponse ToResponse(this Contact contact)
    {
        return new ContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            CEP = contact.CEP,
            Complement = contact.Complement,
            Email = contact.Email,
            Number = contact.Number,
            PersonId = contact.PersonId,
            PhoneNumber = contact.PhoneNumber
        };
    }

    public static Contact ToEntity(this ContactRequest contactRequest)
    {
        return new Contact
        {
            Id = contactRequest.Id,
            PersonId = contactRequest.PersonId,
            Name = contactRequest.Name,
            CEP = contactRequest.CEP,
            Complement = contactRequest.Complement,
            Email = contactRequest.Email,
            Number = contactRequest.Number,
            PhoneNumber = contactRequest.PhoneNumber,
        };
    }
}
