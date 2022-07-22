using APPLICATION.DOMAIN.DTOS.RESPONSE.CONTACT;
using APPLICATION.DOMAIN.ENTITY.CONTACT;

namespace APPLICATION.DOMAIN.UTILS.Extensions;

public static class ContactExtensions
{
    public static ContactResponse ToResponse(this Contact contact)
    {
        return new ContactResponse
        {
            Name = contact.Name,
            CEP = contact.CEP,
            Complement = contact.Complement,
            Email = contact.Email,
            Number = contact.Number,
            PersonId = contact.PersonId,
            PhoneNumber = contact.PhoneNumber
        };
    }
}
